# goto main.py folder open cmd prompt run stream run main.py

import pandas as pd
import re
import nltk
import streamlit as st
from nltk.corpus import stopwords
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import accuracy_score
from sklearn.preprocessing import LabelEncoder
from imblearn.over_sampling import SMOTE
from gtts import gTTS
import os

df = pd.read_csv(r"data/fake_news_dataset.csv")

nltk.download('stopwords')
stop_words = set(stopwords.words('english'))

#Text Cleaning Function
def clean_text(text):
    text = text.lower()
    text = re.sub(r'\W', ' ', text)
    text = re.sub(r'\s+', ' ', text)
    text = ' '.join(word for word in text.split() if word not in stop_words)
    return text
#Apply text cleaning
df['clean_text'] = df['text'].apply(clean_text)

X = df['clean_text']
y = df['label']

#Split dataset
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

tfidf = TfidfVectorizer(max_features=5000)
X_train_tfidf = tfidf.fit_transform(X_train).toarray()
X_test_tfidf = tfidf.transform(X_test).toarray()

label_encoder = LabelEncoder()
y_train = label_encoder.fit_transform(y_train)
y_test = label_encoder.transform(y_test)

smote = SMOTE(sampling_strategy='auto', k_neighbors=1)
X_train_resampled, y_train_resampled = smote.fit_resample(X_train_tfidf, y_train)

model = LogisticRegression()
model.fit(X_train_resampled, y_train_resampled)

y_pred = model.predict(X_test_tfidf)
accuracy = accuracy_score(y_test, y_pred)
st.sidebar.write(f"Model Accuracy: **{accuracy * 100:.2f}%**")


def predict_news(news_text):
    news_text = clean_text(news_text)
    vectorized_text = tfidf.transform([news_text]).toarray()
    prediction = model.predict(vectorized_text)[0]
    return label_encoder.inverse_transform([prediction])[0]

st.title("📰 Fake News Detector")

selected_news = st.selectbox("Select a News Article:", df['text'].tolist())

if st.button("Check News"):
    result = predict_news(selected_news)
    st.write(f"### Prediction: **{result}**")

    # Generate Speech
    tts = gTTS(text=f"The selected news is {result}", lang="en")
    audio_path = "news_audio.mp3"
    tts.save(audio_path)

    st.audio(audio_path, format="audio/mp3", autoplay=True)
    os.remove(audio_path)


Fake News Detector

Overview : This project is a Fake News Detector built using Python, Streamlit, and Machine Learning. It allows users to select a news article from a dataset and determine whether it is Real or Fake using a trained Logistic Regression model. The application also features text-to-speech (TTS), which automatically announces the result after prediction.

Features :
News Selection: Choose a news article from a dropdown.
Fake News Prediction: Uses a trained ML model to classify news as Fake or Real.
Automatic Speech Output: The result is spoken aloud after prediction.
Model Accuracy Display: Shows model accuracy in the sidebar.
Efficient Data Handling: Uses TF-IDF vectorization and SMOTE to handle imbalanced data.

Technologies Used : 
Python (Numpy, Pandas, NLTK, Sklearn, Imbalanced-learn)
Machine Learning (Logistic Regression, TF-IDF, SMOTE)
Streamlit (Web UI for interaction)
gTTS (Google Text-to-Speech for audio output)

Installation & Setup : 

- Prerequisites : Ensure you have Python installed (preferably Python 3.10 or later).
- Install Required Packages - pip install pandas nltk streamlit scikit-learn imbalanced-learn gtts
- Download NLTK Stopwords - import nltk - nltk.download('stopwords')
- Run the Application usin cmd streamlit run main.py

Usage : 
Launch the app with the command above.
Select a news article from the dropdown.
Click 'Check News' to classify it.
The result will be displayed and automatically spoken aloud.

-Model Training & Accuracy
Dataset: Fake news dataset (fake_news_dataset.csv)
Vectorization: TF-IDF (max 5000 features)
Imbalance Handling: SMOTE applied
Model: Logistic Regression
Accuracy: Displayed in Streamlit sidebar

Future Improvements : 
Allow manual text entry for custom news analysis.
Improve ML model by using advanced NLP techniques.
Add more languages for speech output.

Credits : 
Dataset Source: Publicly available Fake News datasets
Libraries: Scikit-learn, Streamlit, gTTS, Pandas, NLTK

License : This project is licensed under the MIT License.
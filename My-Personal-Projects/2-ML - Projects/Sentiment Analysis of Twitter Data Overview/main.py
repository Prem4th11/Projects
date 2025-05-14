import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from wordcloud import WordCloud
import nltk

nltk.download('vader_lexicon')

file_path = r"data\twitter_sentiment_results.csv"
df = pd.read_csv(file_path)

if 'sentiment' not in df.columns:
    raise ValueError("Column 'sentiment' not found! Check the dataset.")

plt.figure(figsize=(7, 5))
sns.countplot(x=df['sentiment'], hue=df['sentiment'],
              palette={'Positive 😊': 'green', 'Neutral 😐': 'gray', 'Negative 😡': 'red'},
              legend=False)
plt.title("Sentiment Distribution of Tweets", fontsize=14)
plt.xlabel("Sentiment", fontsize=12)
plt.ylabel("Tweet Count", fontsize=12)
plt.xticks(fontsize=10)
plt.yticks(fontsize=10)
plt.show()

plt.figure(figsize=(6, 6))
df['sentiment'].value_counts().plot(kind='pie', autopct='%1.1f%%',
                                    colors=['red', 'gray', 'green'],
                                    startangle=90, wedgeprops={'edgecolor': 'black'})
plt.title("Sentiment Breakdown", fontsize=14)
plt.ylabel("")  # Hide y-label
plt.show()

positive_tweets = " ".join(df[df['sentiment'] == 'Positive 😊']['tweet'])
wordcloud = WordCloud(width=800, height=400, background_color="white").generate(positive_tweets)

plt.figure(figsize=(10, 5))
plt.imshow(wordcloud, interpolation="bilinear")
plt.axis("off")
plt.title("Word Cloud of Positive Tweets", fontsize=14)
plt.show()

negative_tweets = " ".join(df[df['sentiment'] == 'Negative 😡']['tweet'])
wordcloud_neg = WordCloud(width=800, height=400, background_color="black", colormap="Reds").generate(negative_tweets)

plt.figure(figsize=(10, 5))
plt.imshow(wordcloud_neg, interpolation="bilinear")
plt.axis("off")
plt.title("Word Cloud of Negative Tweets", fontsize=14)
plt.show()

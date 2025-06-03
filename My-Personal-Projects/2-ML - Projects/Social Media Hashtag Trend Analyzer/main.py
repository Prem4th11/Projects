import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
import re
from collections import Counter
from wordcloud import WordCloud

file_path = r"data\twitter.csv"
df = pd.read_csv(file_path, encoding="latin1", low_memory=False)

tweet_column = None
possible_columns = ["text", "Tweet", "tweet", "content"]

for col in possible_columns:
    if col in df.columns:
        tweet_column = col
        break

if not tweet_column:
    raise ValueError("Could not find a tweet text column in the dataset.")

df[tweet_column] = df[tweet_column].fillna("")
def extract_hashtags(text):
    return re.findall(r"#(\w+)", text)

df["hashtags"] = df[tweet_column].apply(extract_hashtags)

all_hashtags = [tag for sublist in df["hashtags"] for tag in sublist]
hashtag_counts = Counter(all_hashtags)

df.to_csv("hashtag_analysis.csv", index=False)

#Get top 10 trending hashtags
top_hashtags = hashtag_counts.most_common(10)

if not top_hashtags:
    print("No hashtags found in the dataset.")
else:
    tags, counts = zip(*top_hashtags)

    #Bar Chart for Top Hashtags
    plt.figure(figsize=(10, 5))
    sns.barplot(x=list(tags), y=list(counts), hue=list(tags), palette="viridis", legend=False)
    plt.xlabel("Hashtags")
    plt.ylabel("Frequency")
    plt.title("Top Hashtags Trend")
    plt.xticks(rotation=45)
    plt.show()

    #Generate Word Cloud
    wordcloud = WordCloud(width=800, height=400, background_color="black").generate(" ".join(all_hashtags))
    plt.figure(figsize=(10, 5))
    plt.imshow(wordcloud, interpolation="bilinear")
    plt.axis("off")
    plt.title("Hashtag Word Cloud")
    plt.show()

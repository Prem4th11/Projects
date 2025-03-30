Social Media Hashtag Trend Analyzer

Overview : This project analyzes social media trends by extracting and visualizing popular hashtags from a dataset of tweets. It helps identify trending topics, visualize hashtag frequencies, and generate a word cloud for better insights.

Features : 
-Extracts hashtags from tweets
-Counts the frequency of each hashtag
-Identifies the top 10 trending hashtags
-Generates a bar chart to display top hashtags
-Creates a word cloud to visualize hashtag usage
-Saves results to hashtag_analysis.csv

Dependencies :
Ensure you have the following Python libraries installed:
pip install pandas, matplotlib, seaborn, wordcloud,

Dataset Requirements : twitter.csv downlaoded from Kaggle
The dataset should be a CSV file containing tweets
It should have a column named text, Tweet, tweet, or content
Example dataset file path: data/twitter.csv

How It Works :
Loads the Twitter dataset.
Identifies the column containing tweet text.
Extracts all hashtags from tweets.
Counts occurrences of each hashtag.
Saves extracted hashtags to a CSV file.
Displays a bar chart of the most common hashtags.
Creates a word cloud for visualization.

Visualization :
Bar Chart: Shows the frequency of top 10 hashtags.
Word Cloud: Displays hashtags in a visually appealing format.

Usage : 
Run the script in Python:
main.py

Future Enhancements :
Sentiment analysis of tweets
Real-time hashtag tracking using the Twitter API
Time-based hashtag trends
Multi-platform analysis (Instagram, Reddit, etc.)

License : This project is open-source. Feel free to modify and expand it!
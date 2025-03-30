Movie Recommendation Engine/System

Overview :This project is a Movie Recommendation System using Python, Tkinter, and Surprise (SVD Algorithm). The system provides personalized movie recommendations based on user ratings.

Features
Loads movie ratings and movie titles from CSV files.
Uses Singular Value Decomposition (SVD) to predict ratings.
Provides top movie recommendations for a selected user.
User-friendly GUI built with Tkinter.
Supports multi-threading for fast predictions.

Requirements: To run this project, you need to install the following dependencies:
pip install pandas, tkinter, scikit-surprise

Files
ratings.csv - Dataset containing user ratings for movies.
 data sets: The movie ratings and movie details datasets were downloaded from Kaggle.
           movies.csv - Dataset containing movie titles.
           main.py - The main Python script that runs the recommendation system.

How to Run :
Ensure you have ratings.csv and movies.csv in the same directory.
Run the script: python main.py
Select a user ID from the dropdown and click "Get Recommendations".

How It Works :
Load Data: The system loads movie ratings and titles from CSV files.
Train Model: The Surprise SVD model is trained on the rating dataset.
Predict Ratings: The system predicts ratings for all movies the user hasn’t rated.
Recommend Movies: It selects the top-rated movies and displays them in the GUI.

Technologies Used:
Python: Programming Language
Tkinter: GUI Development
Pandas: Data Handling
Surprise: Machine Learning (SVD Algorithm)

ThreadPoolExecutor: Multi-threading for faster recommendations

Screenshot : attached in docs folder

Future Improvements
Implement a more advanced recommendation algorithm.
Allow filtering by genres.
Provide recommendations for new users (Cold Start Problem).

License : This project is open-source and available under the MIT License.
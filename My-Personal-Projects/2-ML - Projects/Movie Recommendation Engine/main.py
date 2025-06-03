import pandas as pd
import tkinter as tk
from tkinter import ttk
from surprise import SVD, Dataset, Reader
from concurrent.futures import ThreadPoolExecutor

#Load datasets
ratings = pd.read_csv(r"data\ratings.csv").sample(10000, random_state=42)
movies = pd.read_csv(r"data\movies.csv")  # Movie titles dataset

#Setup Surprise dataset
reader = Reader(rating_scale=(0.5, 5))
data = Dataset.load_from_df(ratings[['userId', 'movieId', 'rating']], reader)
trainset = data.build_full_trainset()
model = SVD(n_factors=50, random_state=42)
model.fit(trainset)#Train the model once


def recommend_movies(user_id, n=5):
    unique_movies = ratings["movieId"].unique()
    with ThreadPoolExecutor() as executor:
        predictions = list(executor.map(lambda movie: model.predict(user_id, movie), unique_movies))

    #Sort movies by predicted rating
    top_movies = sorted(predictions, key=lambda x: x.est, reverse=True)[:n]

    #Get movie titles
    movie_ids = [pred.iid for pred in top_movies]
    return movies[movies["movieId"].isin(movie_ids)][["movieId", "title"]]

class MovieRecommenderApp:
    def __init__(self, root):
        self.root = root
        self.root.title("🎬 Movie Recommendation System")
        self.root.geometry("550x450")
        self.root.configure(bg="#2C3E50")

        ttk.Label(root, text="🎥 Movie Recommendation System", font=("Arial", 16, "bold"), background="#2C3E50",
                  foreground="white").pack(pady=10)

        ttk.Label(root, text="Select User ID:", font=("Arial", 12), background="#2C3E50", foreground="white").pack(pady=5)
        self.user_id_var = tk.StringVar()
        self.user_dropdown = ttk.Combobox(root, textvariable=self.user_id_var, state="readonly", font=("Arial", 12))
        self.user_dropdown.pack()
        self.user_dropdown["values"] = sorted(ratings["userId"].unique())  #Load User ids
        self.user_dropdown.current(0)  # Set default value

        self.recommend_btn = tk.Button(root, text="🎬 Get Recommendations", command=self.get_recommendations, font=("Arial", 12, "bold"),
                                       bg="#E74C3C", fg="white", padx=10, pady=5, relief="raised")
        self.recommend_btn.pack(pady=15)
        self.result_box = tk.Listbox(root, width=60, height=10, font=("Arial", 12), bg="#ECF0F1", fg="#2C3E50")
        self.result_box.pack(pady=10)

    # Fetch and display recommended movies.
    def get_recommendations(self):
        try:
            user_id = int(self.user_id_var.get())
            recommended = recommend_movies(user_id)
            self.result_box.delete(0, tk.END)

            if not recommended.empty:
                for idx, row in recommended.iterrows():
                    self.result_box.insert(tk.END, f"🎥 {row['title']}")
            else:
                self.result_box.insert(tk.END, "No recommendations found!")
        except Exception as e:
            self.result_box.delete(0, tk.END)
            self.result_box.insert(tk.END, f"Error: {str(e)}")

if __name__ == "__main__":
    root = tk.Tk()
    app = MovieRecommenderApp(root)
    root.mainloop()

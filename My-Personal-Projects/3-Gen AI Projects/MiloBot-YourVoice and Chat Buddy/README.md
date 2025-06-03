# MiloBot — Voice & Text Chatbot

MiloBot is your personal AI chatbot designed to chat with you using both **voice** and **text**. Whether you're feeling emotional, want a fun fact, need motivation, or just want to hear a joke, MiloBot is here to talk.

# Demo Video
- added along with files 

# How It Works

- The chatbot reads data from `data.json`, which contains all possible response categories:
  - Emotions
  - Jokes
  - Facts
  - Science & Math
  - Country Capitals
  - Presidents
  - Motivational Quotes
- Based on your voice or text input, MiloBot selects a matching category and responds accordingly.
- It uses SpeechRecognition for voice input and pyttsx3 for text-to-speech output (desktop compatible).

# Features

-  Voice or text input
-  Smart keyword-based response detection
-  Tells jokes & facts
-  Shares knowledge on countries, science, and math
-  Motivates you when needed
-  Speaks back to you

## Files Included
- app.py  = Main Gradio app logic
- data.json = All chatbot response categories and data
- requirements.txt = Dependencies to run the app
- README.md = The file you're reading

# Run it on Hugging Face or Locally

To run it locally:
```bash
pip install -r requirements.txt
python app.py

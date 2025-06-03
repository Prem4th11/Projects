import gradio as gr
import pyttsx3
import speech_recognition as sr
import json
import random
from datetime import datetime
import os

# Initialize text-to-speech engine
engine = pyttsx3.init()
engine.setProperty('rate', 150)

# Load data from JSON file with UTF-8 encoding to support emojis/special characters
with open("data.json", "r", encoding="utf-8") as file:
    data = json.load(file)

# Function to delete previous audio files to avoid saving
def cleanup_old_audio_files(directory="."):
    for file in os.listdir(directory):
        if file.startswith("response_audio_") and file.endswith(".wav"):
            try:
                os.remove(os.path.join(directory, file))
            except Exception as e:
                print(f"Error deleting file {file}: {e}")

# Function to handle user input (audio or text)
def milobot_response(audio_file, text_input):
    cleanup_old_audio_files()
    response = ""
    audio_output_path = f"response_audio_{datetime.now().strftime('%H%M%S')}.wav"

    # Handling audio input to convert speech to text
    if audio_file:
        recognizer = sr.Recognizer()
        with sr.AudioFile(audio_file) as source:
            audio = recognizer.record(source)
            try:
                user_input = recognizer.recognize_google(audio).lower()
            except sr.UnknownValueError:
                return "Sorry, I couldn't understand the audio. Please try again.", None
    elif text_input:
        user_input = text_input.lower()
    else:
        return "Please provide either text or audio input!", None

    if any(emotion in user_input for emotion in data['emotions'].keys()):
        for emotion in data['emotions']:
            if emotion in user_input:
                response = data['emotions'][emotion]
                break

    elif "weather" in user_input:
        if "today" in user_input:
            today = datetime.now().strftime("%A, %B %d")
            response = data['weather']['today'].format(date=today)
        elif "tomorrow" in user_input:
            response = data['weather']['tomorrow']
        elif "yesterday" in user_input:
            response = data['weather']['yesterday']

    elif any(greeting in user_input for greeting in data['greetings'].keys()):
        for greeting in data['greetings']:
            if greeting in user_input:
                response = data['greetings'][greeting]
                break

    elif "capital" in user_input or "president" in user_input:
        for country in data['countries']:
            if country in user_input:
                if "capital" in user_input:
                    response = f"The capital of {country.capitalize()} is {data['countries'][country]['capital']}."
                elif "president" in user_input:
                    response = f"The president of {country.capitalize()} is {data['countries'][country]['president']}."
                break

    elif "it industry" in user_input or "top it companies" in user_input or "tech companies" in user_input:
        response = "Some of the top IT industries include:\n" + "\n".join(data['it_industries'])

    elif "joke" in user_input:
        response = random.choice(data['jokes'])

    elif "motivate" in user_input or "motivation" in user_input:
        response = random.choice(data['motivation'])

    elif "fun fact" in user_input:
        response = random.choice(data['fun_facts'])

    elif "math fact" in user_input:
        response = random.choice(data['math_facts'])

    elif "science fact" in user_input:
        response = random.choice(data['science_facts'])

    elif "general knowledge" in user_input or "general fact" in user_input:
        response = random.choice(data['general_knowledge'])

    else:
        response = "Sorry, Please try again."

    # Generate the audio response
    engine.save_to_file(response, audio_output_path)
    engine.runAndWait()

    # Return the response and the audio file path
    return response, audio_output_path

# Gradio Interface
iface = gr.Interface(
    fn=milobot_response,
    inputs=[
        gr.Audio(label="Upload Your Voice (Optional)", type="filepath"),
        gr.Textbox(label="Type Your Message (Optional)", placeholder="Type here..."),
    ],
    outputs=[
        gr.Textbox(label="Milo's Response (Text)"),
        gr.Audio(label="Milo's Response (Speech)", type="filepath", autoplay=True),
    ],
    title="MiloBot - Your Voice & Chat Buddy",
    description="Talk to MiloBot using text or voice! Milo understands emotions, weather, jokes, fun facts, and more 🎉"
)


iface.launch()
# iface.launch(share=True)# use this if you want to test it in mobile or locally

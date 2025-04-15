<<<<<<< HEAD
#Audio-to-Text Summarizer
This app uses OpenAI Whisper to transcribe audio files and Google's FLAN-T5 to process that transcription using a custom prompt.

#Features
- Upload `.wav`, `.mp3`, or `.m4a` audio files
- Get transcription and summarization
- Custom prompts like translation, sentiment, etc.

#Run Locally
```bash
pip install -r requirements.txt
python app.py

#challenges faced
- File format errors with .wav (resolved using ffmpeg)
- torchaudio errors (fallback used via soundfile)
- Audio files not recognized (used Hugging Face model-friendly loading)
- FFmpeg not installed (added Windows setup instructions)

#Credits
- Gradio, Transformers, PyTorch
=======
---
title: Audio Text Summarizer
emoji: 🐨
colorFrom: red
colorTo: indigo
sdk: gradio
sdk_version: 5.25.1
app_file: app.py
pinned: false
license: mit
short_description: Convert spoken audio to smart text using Whisper and FLAN-T5
---

Check out the configuration reference at https://huggingface.co/docs/hub/spaces-config-reference
>>>>>>> c5ecbe9df46324011d99e7bf12de8216c9401f86

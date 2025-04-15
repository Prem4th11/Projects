import gradio as gr
import torch
import torchaudio
import soundfile as sf
from transformers import pipeline

# Load Models
asr = pipeline("automatic-speech-recognition", model="openai/whisper-base")

generator = pipeline("text2text-generation", model="google/flan-t5-base")

# Safe Audio Load
def safe_load_audio(audio_path):
    try:
        return torchaudio.load(audio_path)
    except:
        audio, sr = sf.read(audio_path)
        audio_tensor = torch.tensor(audio).unsqueeze(0) if audio.ndim == 1 else torch.tensor(audio).mean(dim=1).unsqueeze(0)
        return audio_tensor, sr

# Inference Logic
def audio_text_to_text(audio_path, prompt):
    try:

        # Load
        audio_tensor, sr = safe_load_audio(audio_path)
        if sr != 16000:
            audio_tensor = torchaudio.transforms.Resample(sr, 16000)(audio_tensor)
            sr = 16000

        if audio_tensor.shape[0] > 1:
            audio_tensor = audio_tensor.mean(dim=0, keepdim=True)

        # Transcription
        result = asr({"array": audio_tensor.squeeze().numpy(), "sampling_rate": sr})
        transcription = result["text"]

        # Generate Summary or Other
        final_prompt = f"{prompt}\n\nAudio says: {transcription}"
        summary = generator(final_prompt, max_length=100)[0]["generated_text"]
        return summary

    except Exception as e:
        return f"Error: {str(e)}"

# UI
interface = gr.Interface(
    fn=audio_text_to_text,
    inputs=[
        gr.Audio(type="filepath", label="Upload Audio (.mp3, .m4a, .wav)"),
        gr.Textbox(label="Enter a prompt (e.g., 'Summarize this')")
    ],
    outputs="text",
    title=" Audio-to-Text Summarizer",
    description="Upload audio and prompt like 'Summarize this' or 'Translate this to French'."
)

# Run
interface.launch()
#!/usr/bin/env python3
# communication.py
# Solomon Himelbloom, Jacob Jakiemiec, Andrew Ng, & Travis Winterton
# 2023-02-02

"""
Source file for function project_description.
Demonstrates morse code (https://en.wikipedia.org/wiki/Morse_code) in Python.
"""

import morse_talk as mtalk
import pyttsx3

version_number = "2023"


def introduction():
    """
    At program startup, read the default welcome message.
    The software version is in YYYY format.
    """
    pyttsx3.speak("Virtual Morse - Version" + version_number)


def project_description(input):
    """First attempt at morse code in Python."""
    sample = mtalk.encode(input)
    print(f"Example program run: {sample}")


def reverse_morse():
    """Reverse morse code."""
    command_mode = True

    while command_mode:
        command = input("Enter morse code: ")
        if command == "exit":
            command_mode = False
        else:
            print(mtalk.decode(command))


def tts():
    """Text to speech."""
    # ?: Add text to speech.
    sample_text = "This is a samle of text-to-speech running with Python."
    pyttsx3.speak(sample_text)


def switch_input():
    """Connection betweeen keyboard input and morse code output."""
    # ?: Add keyboard input.
    pass


if __name__ == "__main__":
    message = [
        "Hello, world.",
        "UAF CS Spring 2023",
        "Computer Science Capstone Project",
        "Assistive Technology",
    ]

    introduction()
    # project_description(message[0])
    # reverse_morse()
    tts()

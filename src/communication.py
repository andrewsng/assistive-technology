#!/usr/bin/env python3
# communication.py
# Solomon Himelbloom, Jacob Jakiemiec, Andrew Ng, & Travis Winterton
# 2023-02-02

"""
Source file for function project_description.
Demonstrates morse code in Python.
"""

import morse_talk as mtalk
from tkinter import *
from tkinter import ttk


def project_description(input):
    """First attempt at morse code in Python."""
    sample = mtalk.encode(input)
    print(f"Example program run: {sample}")


def new_input_window():
    """Spawn a Notepad to record user input."""
    example_text = message[1] + " → " + message[2]
    print(f"Example program run: {example_text}")

    root = Tk()
    root.title("Virtual Morse 2023")
    root.geometry("500x500")

    frm = ttk.Frame(root, padding=10)
    frm.grid()
    ttk.Label(frm, text=example_text).grid(row=0, column=0, sticky="w")
    root.mainloop()


if __name__ == "__main__":
    message = [
        "Hello, world.",
        "UAF CS Spring 2023",
        "Computer Science Capstone Project",
        "Assistive Technology",
    ]

    new_input_window()

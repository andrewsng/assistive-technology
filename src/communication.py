#!/usr/bin/env python3
# communication.py
# Solomon Himelbloom, Jacob Jakiemiec, Andrew Ng, & Travis Winterton
# 2023-02-02

"""
Source file for function project_description.
Demonstrates morse code in Python.
"""

import morse_talk as mtalk


def project_description(input):
    """First attempt at morse code in Python."""
    sample = mtalk.encode(input)
    print(f"Example program run: {sample}")


if __name__ == "__main__":
    message = [
        "Hello, world.",
        "UAF CS Spring 2023",
        "Computer Science Capstone Project",
        "Assistive Technology",
    ]

    # TODO: Specify case-sensitive input?
    project_description(message[2])

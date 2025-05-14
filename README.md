# Cybersecurity Awareness Chatbot

This is a C# console-based chatbot designed to simulate a conversational assistant that provides cybersecurity advice. The project was developed as part of a programming assignment to demonstrate the use of object-oriented programming, delegates, interface abstraction, conversation flow, and sentiment detection.

---

## Features

### Task 1: Basic Chatbot Structure
- Greets the user by name
- Provides a looping conversation system
- Exits cleanly with a farewell message

### Task 2: Keyword Recognition with Delegates
- Uses a `ResponseDelegate` to separate chatbot logic
- Detects keywords like **password**, **phishing**, **malware**, and more
- Each keyword has multiple predefined responses for variety

### Task 3: Improved Conversation Flow
- Implements a simple state tracker using `enum`
- Enhances conversation flow to make interactions more natural

### Task 4: Logging and User Profile
- Stores user details such as name
- Logs each conversation entry to a file
- Tracks session duration and keywords used

### Task 5: Sentiment Detection
- Identifies emotions like **curious**, **worried**, **frustrated**, and **confused**
- Returns empathetic responses from a `List<string>` based on detected sentiment

### Task 6: (Optional / Future Expansion)
- Could include question prediction, natural language processing, or basic learning ability

---

##  Technologies Used

- **Language**: C#
- **Framework**: .NET Core / .NET 6+
- **IDE**: Visual Studio / Visual Studio Code
- **Console App**: Text-based interface using ANSI color formatting

---

##  How to Run

1. Clone or download the project folder.
2. Open in Visual Studio or your preferred C# editor.
3. Build the solution.
4. Run the project. Type in questions related to cybersecurity.
5. Type `exit` to close the chatbot.

---

##  Sample Inputs

You: I’m worried someone hacked me
Bot: It's okay to feel concerned—you're not alone.

You: How can I protect my password?
Bot: A strong password includes a mix of letters, numbers, and special characters.

You: I'm confused about phishing
Bot: No worries—let me clarify that for you.


---

##  Log File Location

The log file is automatically generated in the project directory with a `.txt` extension. It stores each conversation with timestamps.

---

##  Author

Created by [Your Name]  
Project for [Your Institution or Class Name]  
Date: [Month Year]

---

##  License

This project is licensed for educational purposes.

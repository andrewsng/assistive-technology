// Josh Board Arduino Code
// Use for handling signals from josh's swtich interface 
// Signal sent by serial monitor to virutal mores

//Array of ints corresponding to digitalPins on Arduino
const int buttonPins[] = {2, 3, 4, 5, 6, 7, 8, 9, 10}; 

const int numButtons = sizeof(buttonPins)/sizeof(buttonPins[0]); // calculate the number of buttons

int buttonStates[numButtons];

// Initialize all pins in ButtonPins[]
// Initialize serial communication
void setup() {
  Serial.begin(9600);  
  for (int i = 0; i < numButtons; i++) {
    pinMode(buttonPins[i], INPUT_PULLUP);
  }
}

void loop() {
  for (int i = 0; i < numButtons; i++) {
    buttonStates[i] = digitalRead(buttonPins[i]);  // read the button state
    if (buttonStates[i] == LOW) {
      Serial.println(i);
      while (digitalRead(buttonPins[i]) == LOW) {  // wait for the button to be released
        delay(10);  // add a small delay to reduce the loop frequency
      }
    }
  }
}

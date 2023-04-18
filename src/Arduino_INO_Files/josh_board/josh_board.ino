const int buttonPins[] = {2, 3, 4, 5, 6, 7, 8, 9, 10}; 

const int numButtons = sizeof(buttonPins)/sizeof(buttonPins[0]); // calculate the number of buttons

int buttonStates[numButtons];

void setup() {
  Serial.begin(9600);  // initialize serial communication
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

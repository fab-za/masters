int fine = round(1000000/300);
int mid = round(1000000/250);
int coarse = round(1000000/200);

int hapticPin = 9;

void setup() {
  Serial.being(9600);

  pinMode(hapticPin, OUTPUT);

}

void loop() {
  // put your main code here, to run repeatedly:
  char c = Serial.read();

  if(c == 'C'){vibrate(hapticPin, fine);}
  else if(c == 'D'){vibrate(hapticPin, mid);}
  else if(c == 'E'){vibrate(hapticPin, coarse);}
}

void vibrate(int pin, int f){
  digitalWrite(pin, HIGH);
  delayMicroseconds(f/2);
  digitalWrite(pin, LOW);
  delayMicroseconds(f/2);
}

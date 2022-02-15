int fine = round(1000000/300);
int mid = round(1000000/200);
int coarse = round(1000000/130);

int hapticPin = 9;

long elapsedtime = 0;
long duration = 8000000;

void setup() {
  Serial.begin(9600);

  pinMode(hapticPin, OUTPUT);

}

void loop() {
  testVibrate();
  
//  char c = Serial.read();
//
//  if(c == 'C'){vibrate(hapticPin, fine);}
//  else if(c == 'D'){vibrate(hapticPin, mid);}
//  else if(c == 'E'){vibrate(hapticPin, coarse);}
}

void vibrate(int pin, int f){
  digitalWrite(pin, HIGH);
  delayMicroseconds(f/2);
  digitalWrite(pin, LOW);
  delayMicroseconds(f/2);
}

void testVibrate(){
  while(elapsedtime < duration){
    vibrate(hapticPin, fine);
    elapsedtime += fine;
    }
  elapsedtime = 0;
  
  while(elapsedtime < duration){
    vibrate(hapticPin, mid);
    elapsedtime += mid;
    }
  elapsedtime = 0;
  
  while(elapsedtime < duration){
    vibrate(hapticPin, coarse);
    elapsedtime += coarse;
    }
  elapsedtime = 0;
}

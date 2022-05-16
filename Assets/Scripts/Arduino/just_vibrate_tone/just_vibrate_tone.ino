#include "pitches.h"

int hapticPin = 8;

long elapsedtime = 0;
long duration = 50000000;
int toneDuration = 500;

int train1 = 300;
int train2 = 275;
int train3 = 250;

int test1 = 280;
int test2 = 260;
int test3 = 290;
int test4 = 270;

void setup() {
  Serial.begin(9600);

  pinMode(hapticPin, OUTPUT);

  testVibrate();

}

void loop() {
  char c = Serial.read();

  if(c == 'A'){tone(hapticPin, train2, toneDuration);}

  else if(c == 'C'){tone(hapticPin, train1, toneDuration);}
  else if(c == 'D'){tone(hapticPin, train2, toneDuration);}
  else if(c == 'E'){tone(hapticPin, train3, toneDuration);}
  
  else if(c == 'F'){tone(hapticPin, test1, toneDuration);}
  else if(c == 'G'){tone(hapticPin, test2, toneDuration);}
  else if(c == 'H'){tone(hapticPin, test3, toneDuration);}
  else if(c == 'I'){tone(hapticPin, test4, toneDuration);}
}

void vibrate(int pin, int f){
  digitalWrite(pin, HIGH);
  delayMicroseconds(f/2);
  digitalWrite(pin, LOW);
  delayMicroseconds(f/2);
}

void testVibrate(){
  while(elapsedtime < duration){
    vibrate(hapticPin, train1);
    elapsedtime += train1;
    }
  elapsedtime = 0;

  delay(500);
  
  while(elapsedtime < duration){
    vibrate(hapticPin, train2);
    elapsedtime += train2;
    }
  elapsedtime = 0;
  
  delay(500);
  
  while(elapsedtime < duration){
    vibrate(hapticPin, train3);
    elapsedtime += train3;
    }
  elapsedtime = 0;
}

int hapticPin = 9;

long elapsedtime = 0;
long duration = 8000000;

int train1 = round(1000000/130);
int train2 = round(1000000/215);
int train3 = round(1000000/300);

int test1 = round(1000000/280);
int test2 = round(1000000/150);
int test3 = round(1000000/260);
int test4 = round(1000000/180);

void setup() {
  Serial.begin(9600);

  pinMode(hapticPin, OUTPUT);

//  testVibrate();

}

void loop() {
  char c = Serial.read();

  if(c == 'A'){vibrate(hapticPin, train2);}

  else if(c == 'C'){vibrate(hapticPin, train1);}
  else if(c == 'D'){vibrate(hapticPin, train2);}
  else if(c == 'E'){vibrate(hapticPin, train3);}
  
  else if(c == 'F'){vibrate(hapticPin, test1);}
  else if(c == 'G'){vibrate(hapticPin, test2);}
  else if(c == 'H'){vibrate(hapticPin, test3);}
  else if(c == 'I'){vibrate(hapticPin, test4);}
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
  
  while(elapsedtime < duration){
    vibrate(hapticPin, train2);
    elapsedtime += train2;
    }
  elapsedtime = 0;
  
  while(elapsedtime < duration){
    vibrate(hapticPin, train3);
    elapsedtime += train3;
    }
  elapsedtime = 0;
}


int hapticPin_left = 11;
int hapticPin_right = 10;

//-------- VARIABLES ---------------------

char buf[4];

int train1 = 300;
int train2 = 275;
int train3 = 250;

int test1 = 280;
int test2 = 260;
int test3 = 290;
//int test4 = 270;
//int test5 = 270;

long elapsedtime = 0;
long duration = 100000;

int vibrationMode_left = 0;
int vibrationMode_right = 0;

void setup() {
  pinMode(hapticPin_left, OUTPUT);
  pinMode(hapticPin_right, OUTPUT);

  Serial.begin(9600);
  Serial.setTimeout(200);

  longVibrate(hapticPin_left, 275);
  longVibrate(hapticPin_right, 275);
}

void loop() {
  checkMessage();
  
  while(vibrationMode_left > 0 && vibrationMode_right > 0){
    checkMessage();
    vibrate(hapticPin_left, vibrationMode_left);
    vibrate(hapticPin_right, vibrationMode_right);
  }
}

void vibrate(int pin, int f){
  digitalWrite(pin, HIGH);
  delayMicroseconds(f/2);
  digitalWrite(pin, LOW);
  delayMicroseconds(f/2);
}

void longVibrate(int pin, int f){
  while(elapsedtime < duration){
    vibrate(pin, f);
    elapsedtime += f;
    }
  elapsedtime = 0;
}


void callVibrate(int pin, char mode){
  int f = vibrationModeToFrequency(mode);
  if(f > 0){
//    longVibrate(pin, f);
    vibrate(pin, f);
  }
}

int vibrationModeToFrequency(char c) {
  int frequency;

  if(c == 'A'){frequency = 0;}
  
  else if(c == 'B'){frequency = train1;}  
  else if(c == 'C'){frequency = train2;}
  else if(c == 'D'){frequency = train3;}
  
  else if(c == 'E'){frequency = test1;}
  else if(c == 'F'){frequency = test2;}
  else if(c == 'G'){frequency = test3;}
//  else if(c == 'H'){frequency = test4;}
//  else if(c == 'I'){frequency = test5;}

  return frequency;
}

void checkMessage(){
  String message = Serial.readStringUntil('\n');

  if(message != ""){
    message.toCharArray(buf, 4);
    vibrationMode_left = vibrationModeToFrequency(buf[2]);
    vibrationMode_right = vibrationModeToFrequency(buf[3]);
    }
}

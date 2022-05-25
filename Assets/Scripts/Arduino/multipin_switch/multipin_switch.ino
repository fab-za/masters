int pins[2][5] = {{5,4,3,2,A5}, {A4,A3,A2,A1,A0}};
//int pins[2][5] = {{A0,A1,A2,A3,A4},{A5,2,3,4,5}};
//int pins[1][8] = {A0,A1,A2,A3,A4,A5,2,3};

int frequency = 200;
unsigned long check;
unsigned long elapsedtime;

long frequencyMode_left = 1;
long frequencyMode_right = 5;

void setup() {
  for(int i=0; i<5; i++)  {pinMode(pins[0][i], OUTPUT);}
  for(int i=0; i<5; i++)  {pinMode(pins[1][i], OUTPUT);}
  
  Serial.begin(9600);

  check = millis();
}

void loop() {
  elapsedtime = millis() - check;
//  intToBinary(elapsedtime/100, 5, 1);

//  if(elapsedtime > 2000){
//    if(frequency == 80){
//      frequency = 200;
//    }
//    else if(frequency == 200){
//      frequency = 80;
//    }
//    
//    check = millis();
//  }
//  
//  intToBinary(frequency);

  if(elapsedtime > 2000){
    if(frequencyMode_left == 1){
      frequencyMode_left = 30;
    }
    else if(frequencyMode_left == 30){
      frequencyMode_left = 1;
    }

    if(frequencyMode_right == 5){
      frequencyMode_right = 20;
    }
    else if(frequencyMode_right == 20){
      frequencyMode_right = 5;
    }
    
    check = millis();
  }

  intToBinary(frequencyMode_left, 5, 0);
  intToBinary(frequencyMode_right, 5, 1);
}

void intToBinary(int f, int s, int side){
  bool buf[s];
  byte fb = f;
  
  for(int i=(s-1); i>=0; i--)
  {
    bool m = bitRead(f, i);
    buf[i] = m;
  }

  printbuf(buf, s);
  toPin(buf, s, side);
}

void printbuf(bool buf[], int s){
  for(int i=(s-1); i>=0; i--){
    Serial.print(buf[i]);
  }
  Serial.println(" ");
}

void toPin(bool buf[], int s, int side){
  for(int i=(s-1); i>=0; i--){
    digitalWrite(pins[side][i], buf[i]);
  }
}

// --------------- 

void binaryMorse(bool buf[]){
  for(int i=7; i>=0; i--){
    digitalWrite(pins[0], buf[i]);
    delay(5);
  }
}

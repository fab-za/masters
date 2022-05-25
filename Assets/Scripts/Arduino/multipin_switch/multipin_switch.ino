int pins[] = {3,2,A5,A4,A3,A2,A1,A0};

int frequency = 200;
unsigned long check;
unsigned long elapsedtime;

void setup() {
  for(int i=0; i<8; i++)  {pinMode(pins[i], OUTPUT);}
//  Serial.begin(9600);
  check = millis();
}

void loop() {
  elapsedtime = millis() - check;
  if(elapsedtime > 2000){
    if(frequency == 80){
      frequency = 200;
    }
    else if(frequency == 200){
      frequency = 80;
    }
    
    check = millis();
  }
  
  intToBinary8(frequency);
//  intToBinary8(elapsedtime/100);
}

void intToBinary8(int f){
  bool buf[8];
  byte fb = f;
  
  for(int i=7; i>=0; i--)
  {
    bool m = bitRead(f, i);
    buf[i] = m;
  }
  toPin(buf);
}

void printbuf(bool buf[]){
  for(int i=7; i>=0; i--){
//    Serial.print(buf[i]);
  }
//  Serial.println(" ");
}

void binaryMorse(bool buf[]){
  for(int i=7; i>=0; i--){
    digitalWrite(pins[0], buf[i]);
    delay(5);
  }
}

void toPin(bool buf[]){
  for(int i=7; i>=0; i--){
    digitalWrite(pins[i], buf[i]);
  }
}

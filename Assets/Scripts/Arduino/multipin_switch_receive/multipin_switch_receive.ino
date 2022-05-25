int pins[] = {3,2,A5,A4,A3,A2,A1,A0};
//int pins[] = {A0,A1,A2,A3,A4,A5,2,3};

void setup() {
  for(int i=0; i<8; i++)  {pinMode(pins[i], INPUT);}
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.begin(9600);

}

void loop() {
  long frequency = readToBuffer();
  Serial.print(millis());
  Serial.print(", ");
  Serial.println(frequency);

}

long binary8ToInt(bool buf[]){
  long decimal = (buf[7]*128) + (buf[6]*64) + (buf[5]*32) + (buf[4]*16) + (buf[3]*8) + (buf[2]*4) + (buf[1]*2) + (buf[0]*1);
  return decimal;
}

long readToBuffer(){
  bool buf[8];
  
  for(int i=0; i<8; i++){
    bool current = digitalRead(pins[i]);
    buf[i] = current;
  }

  long dec = binary8ToInt(buf);

  return dec;
}

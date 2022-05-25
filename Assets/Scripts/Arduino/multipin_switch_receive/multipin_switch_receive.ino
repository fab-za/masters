//int pins[] = {3,2,A5,A4,A3,A2,A1,A0};
//int pins[] = {A0,A1,A2,A3,A4,A5,2,3};
int pins[2][5] = {{5,4,3,2,A5}, {A4,A3,A2,A1,A0}};

int frequencyMode_left = 1;
int frequencyMode_right = 1;

unsigned long check;

void setup() {
  for(int i=0; i<5; i++)  {pinMode(pins[0][i], INPUT);}
  for(int i=0; i<5; i++)  {pinMode(pins[1][i], INPUT);}
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.begin(9600);

}

void loop() {
  check = millis();
  long frequencyMode_left = readToBuffer(5, 0);
  long frequencyMode_right = readToBuffer(5, 1);
  
  Serial.println(millis()-check);
//  Serial.print(", ");
//  Serial.print(frequencyMode_left);
//  Serial.print(", ");
//  Serial.println(frequencyMode_right);

}

long binary8ToInt(bool buf[]){
  long decimal = (buf[7]*128) + (buf[6]*64) + (buf[5]*32) + (buf[4]*16) + (buf[3]*8) + (buf[2]*4) + (buf[1]*2) + (buf[0]*1);
  return decimal;
}

long binary4ToInt(bool buf[]){
  long decimal = (buf[4]*16) + (buf[3]*8) + (buf[2]*4) + (buf[1]*2) + (buf[0]*1);
  return decimal;
}

long readToBuffer(int s, int side){
  bool buf[s];
  
  for(int i=0; i<s; i++){
    bool current = digitalRead(pins[side][i]);
    buf[i] = current;
  }

//  long dec = binary8ToInt(buf);
  long dec = binary4ToInt(buf);
//  printbuf(buf, 5);

  return dec;
}

void printbuf(bool buf[], int s){
  for(int i=(s-1); i>=0; i--){
    Serial.print(buf[i]);
  }
  Serial.println(" ");
}

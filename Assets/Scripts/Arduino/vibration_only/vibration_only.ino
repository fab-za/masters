
int hapticPin_left = 11;
int hapticPin_right = 10;

int inputPins[2][5] = {{5,4,3,2,A5}, {A4,A3,A2,A1,A0}};

//-------- VARIABLES ---------------------

char buf[5];

long frequency_left = 0;
long frequency_right = 0;
long frequencyMode_left = 0;
long frequencyMode_right = 0;

long elapsedtime = 0;
//long duration = 30;
long milliduration = 2000;

unsigned long startPeriod_left = 0;
unsigned long startPeriod_right = 0;

int leftVal = LOW;
int rightVal = LOW; 

long period_left;
long period_right;

String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

int train1 = 20;
int train2 = 110;
int train3 = 200;

int baseline = 110;
float percent = 0.03;

int test1 = 280;
int test2 = 260;
int test3 = 290;
//int test4 = 270;
//int test5 = 270;

void setup() {
  pinMode(hapticPin_left, OUTPUT);
  pinMode(hapticPin_right, OUTPUT);

  for(int i=0; i<5; i++)  {pinMode(inputPins[0][i], INPUT);}
  for(int i=0; i<5; i++)  {pinMode(inputPins[1][i], INPUT);}

  pinMode(LED_BUILTIN, OUTPUT);

  testVibrate(hapticPin_left, 100);
  testVibrate(hapticPin_right, 100);

  Serial.begin(9600);
}

void loop() {
  frequencyMode_left = readToBuffer(5, 0);
  frequencyMode_right = readToBuffer(5, 1);

  frequency_left = vibrationModeToFrequency(frequencyMode_left);
  frequency_right = vibrationModeToFrequency(frequencyMode_right);

//  Serial.println(millis());
//  Serial.println(frequency_left);

//  long period_left = 1000/(frequency_left);
//  long period_right = 1000/(frequency_right);
//
//  vibrateBoth(period_left, period_right);

  period_left = 1000/frequency_left;
  period_right = 1000/frequency_right;
  
  if(frequency_left > 0 && frequency_right > 0){
    vibrateBoth(period_left, period_right);
    }
  else{
    digitalWrite(hapticPin_left, LOW);
    digitalWrite(hapticPin_right, LOW);
    }
}

// ---------- SUB FUNCTIONS ------------------------
void vibrateBoth(long period_left, long period_right){
  if((millis() - startPeriod_left) > (period_left/2)){
        
    if(leftVal == LOW){
      leftVal = HIGH;
      } 
    else{
      leftVal = LOW;
      }
    
    digitalWrite(hapticPin_left, leftVal);
    startPeriod_left = millis();
  }

  if((millis() - startPeriod_right) > (period_right/2)){
    
    if(rightVal == LOW){
      rightVal = HIGH;
      } 
    else{
      rightVal = LOW;
      }

    digitalWrite(hapticPin_right, rightVal);
    startPeriod_right = millis();
  }
}

void testVibrate(int pin, int f){
  int period = (1000/f);
  while(elapsedtime < milliduration){
    digitalWrite(pin, HIGH);
    delay(period/2);
    digitalWrite(pin, LOW);
    delay(period/2);
    elapsedtime += f;
    }
  elapsedtime = 0;
}

long binary4ToInt(bool buf[]){
  long decimal = (buf[4]*16) + (buf[3]*8) + (buf[2]*4) + (buf[1]*2) + (buf[0]*1);
  return decimal;
}

long readToBuffer(int s, int side){
  bool buf[s];
  
  for(int i=0; i<s; i++){
    bool current = digitalRead(inputPins[side][i]);
    buf[i] = current;
  }
  
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

int vibrationModeToFrequency(long m) {
  int frequency;
  char c = alphabet.charAt(m);

  if(c == 'A'){frequency = 0;}
  
  else if(c == 'C'){frequency = train1;}  
  else if(c == 'G'){frequency = train2;}
  else if(c == 'L'){frequency = train3;}

  else if(c == 'B'){frequency = baseline * (1+(percent*1));}  
  else if(c == 'D'){frequency = baseline * (1+(percent*2));}
  else if(c == 'E'){frequency = baseline * (1+(percent*3));}
  else if(c == 'F'){frequency = baseline * (1+(percent*4));}
  else if(c == 'H'){frequency = baseline * (1+(percent*5));}
  else if(c == 'I'){frequency = baseline * (1+(percent*6));}
  else if(c == 'J'){frequency = baseline * (1+(percent*7));}
  else if(c == 'K'){frequency = baseline * (1+(percent*8));}
  else if(c == 'M'){frequency = baseline * (1+(percent*9));}
  else if(c == 'N'){frequency = baseline * (1+(percent*10));}
  else if(c == 'O'){frequency = baseline * (1+(percent*11));}
  else if(c == 'P'){frequency = baseline * (1+(percent*12));}
  else if(c == 'Q'){frequency = baseline * (1+(percent*13));}
  else if(c == 'R'){frequency = baseline * (1+(percent*14));}
  else if(c == 'U'){frequency = baseline * (1+(percent*15));}
  else if(c == 'V'){frequency = baseline * (1+(percent*16));}
  else if(c == 'W'){frequency = baseline * (1+(percent*17));}
  else if(c == 'X'){frequency = baseline * (1+(percent*18));}
  else if(c == 'Y'){frequency = baseline * (1+(percent*19));}
  else if(c == 'Z'){frequency = baseline * (1+(percent*20));}

  return frequency;
}

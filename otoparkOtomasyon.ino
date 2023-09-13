#include <DHT.h>

#define atesSensor A0
#define isikSensor A1
#define SicaklikSensor 2
#define echoPin 3
#define triggerPin 4
#define echoPin2 5
#define triggerPin2 6
#define arabaKontrol3 7
#define arabaKontrol4 8
#define arabaKontrol5 9
#define arabaKontrol6 10
#define buzzer 11
#define ledAtes 12
#define ledMotorIsik 13

String okunandeger;
int atesValue = 1000;
int isikValue = 1000;
float sicaklikValue = 0;
int fanDurum = 0;
int isikDurum = 0;
int yanginDurum = 0;
int fanDurumGelen = 0;
int isikDurumGelen = 0;
int yanginDurumGelen = 0;
int saniye = 0;
int arabaKontrol1Dolu = 0;
int arabaKontrol2Dolu = 0;
int arabaKontrol3Dolu = 0;
int arabaKontrol4Dolu = 0;
int arabaKontrol5Dolu = 0;
int arabaKontrol6Dolu = 0;
float totalSure;
float mesafe;
float totalSure2;
float mesafe2;
float h = 0;
float t = 0;
float hic = 0;
bool sicaklikOku = true;
bool girisKapi = false;
bool cikisKapi = false;


#define DHTTYPE DHT11
DHT dht(SicaklikSensor, DHTTYPE);

void setup() {
  Serial.begin(9600);
  dht.begin();
  pinMode(atesSensor, INPUT);
  pinMode(SicaklikSensor, INPUT);
  pinMode(isikSensor, INPUT);
  pinMode(ledMotorIsik, OUTPUT);
  pinMode(ledAtes, OUTPUT);
  pinMode(buzzer, OUTPUT);
  pinMode(triggerPin, OUTPUT);
  pinMode(echoPin, INPUT);
  pinMode(triggerPin2, OUTPUT);
  pinMode(echoPin2, INPUT);
  pinMode(arabaKontrol3, INPUT);
  pinMode(arabaKontrol4, INPUT);
  pinMode(arabaKontrol5, INPUT);
  pinMode(arabaKontrol6, INPUT);

  cli();
  TCCR1A = 0;
  TCCR1B = 0;
  TCNT1 = 0;
  OCR1A = 15624;
  TCCR1B |= (1 << WGM12);
  TCCR1B |= (1 << CS12) | (1 << CS10);
  TIMSK1 |= (1 << OCIE1A);
  sei();
}

ISR(TIMER1_COMPA_vect) {
  saniye++;
  Serial.println("Yangin: " + String(yanginDurum) + "-Isik: " + String(isikDurum) + "-Sicaklik: " + String(t) + "-Nem: " + String(h) + "-A1: " + String(arabaKontrol1Dolu) + "-A2: " + String(arabaKontrol2Dolu) + "-A3: " + String(arabaKontrol3Dolu) + "-A4: " + String(arabaKontrol4Dolu) + "-A5: " + String(arabaKontrol5Dolu) + "-A6: " + String(arabaKontrol6Dolu) + "-Fan: " + String(fanDurum));
  if (saniye % 5 == 0) {
    digitalWrite(triggerPin, HIGH);
    delayMicroseconds(1000);
    digitalWrite(triggerPin, LOW);
    totalSure = pulseIn(echoPin, HIGH);
    mesafe = (totalSure / 2.0) / (29.1);
    digitalWrite(triggerPin2, HIGH);
    delayMicroseconds(1000);
    digitalWrite(triggerPin2, LOW);
    totalSure2 = pulseIn(echoPin2, HIGH);
    mesafe2 = (totalSure2 / 2.0) / (29.1);
    if (digitalRead(arabaKontrol3) == 1) {
      arabaKontrol3Dolu = 1;
    } else {
      arabaKontrol3Dolu = 0;
    }
    if (digitalRead(arabaKontrol4) == 1) {
      arabaKontrol4Dolu = 1;
    } else {
      arabaKontrol4Dolu = 0;
    }
    if (digitalRead(arabaKontrol5) == 1) {
      arabaKontrol5Dolu = 1;
    } else {
      arabaKontrol5Dolu = 0;
    }
    if (digitalRead(arabaKontrol6) == 1) {
      arabaKontrol6Dolu = 1;
    } else {
      arabaKontrol6Dolu = 0;
    }
  }
  if (saniye % 10 == 0) {
    sicaklikOku = true;
  }
  if (saniye % 15 == 0) {
    isikValue = analogRead(isikSensor);
  }
  if (saniye % 20 == 0) {
    atesValue = analogRead(atesSensor);
    saniye = 0;
  }
}

void loop() {
  delay(200);
  if (Serial.available() > 0) {
    okunandeger = Serial.readString();
    if (okunandeger.charAt(0) == '0') {
      fanDurumGelen = String(okunandeger.charAt(2)).toInt();
    }
    if (okunandeger.charAt(0) == '1') {
      isikDurumGelen = String(okunandeger.charAt(2)).toInt();
    }
    if (okunandeger.charAt(0) == '2') {
      yanginDurumGelen = String(okunandeger.charAt(2)).toInt();
    }
    if (okunandeger.charAt(0) == '3') {
      girisKapi = true;
    }
    if (okunandeger.charAt(0) == '4') {
      cikisKapi = true;
    }
  }

  if (girisKapi) {
    digitalWrite(buzzer, 300);
    delay(1000);
    girisKapi = false;
    digitalWrite(buzzer, 0);
  }
  if (cikisKapi) {
    analogWrite(buzzer, 100);
    delay(1000);
    cikisKapi = false;
    digitalWrite(buzzer, 0);
  }
  if (sicaklikOku) {
    h = dht.readHumidity();
    t = dht.readTemperature();
    hic = dht.computeHeatIndex(t, h, false);
    sicaklikValue = hic;
    if (!isnan(hic)) {
      sicaklikOku = false;
    }
  }
  if (sicaklikValue > 30 || fanDurumGelen == 1) {
    digitalWrite(ledMotorIsik, HIGH);
    delay(1000);
    digitalWrite(ledMotorIsik, LOW);
    fanDurum = 1;
  } else {
    digitalWrite(ledMotorIsik, LOW);
    fanDurum = 0;
  }
  if (mesafe <= 50) {
    arabaKontrol1Dolu = 1;
  } else {
    arabaKontrol1Dolu = 0;
  }
  if (mesafe2 <= 50) {
    arabaKontrol2Dolu = 1;
  } else {
    arabaKontrol2Dolu = 0;
  }

  if (isikValue < 200 || isikDurumGelen == 1) {
    digitalWrite(ledMotorIsik, HIGH);
    isikDurum = 1;
  } else {
    digitalWrite(ledMotorIsik, LOW);
    isikDurum = 0;
  }
  if (atesValue < 100 || yanginDurumGelen == 1) {
    yanginDurum = 1;
    analogWrite(buzzer, 500);
    digitalWrite(ledAtes, HIGH);
    delay(300);
    analogWrite(buzzer, 0);
    digitalWrite(ledAtes, LOW);
  } else {
    analogWrite(buzzer, 0);
    digitalWrite(ledAtes, LOW);
    yanginDurum = 0;
  }
}
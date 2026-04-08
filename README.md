## Refaktoryzacja

W ramach zadania przeprowadziłem refaktoryzację aplikacji LegacyRenewalApp, zachowując istniejącą funkcjonalność oraz publiczny kontrakt wykorzystywany przez projekt kliencki.

### Co zostało zrealizowane:

- Podzieliłem odpowiedzialności w klasie SubscriptionRenewalService zgodnie z zasadą SRP
- Wydzieliłem logikę biznesową do osobnych klas w warstwie Domain (np. obliczanie podatku, opłat i rabatów)
- Zastąpiłem rozbudowane instrukcje if-else wzorcem strategii (Discount Strategies), zgodnie z Open/Closed Principle
- Ograniczyłem sprzężenie poprzez wprowadzenie abstrakcji IBillingGateway i opakowanie klasy LegacyBillingGateway
- Wprowadziłem prosty mechanizm wstrzykiwania zależności (Dependency Injection)
- Poprawiłem czytelność, testowalność oraz rozszerzalność kodu

Refaktoryzacja nie zmienia zachowania systemu — wyniki działania pozostają takie same jak przed zmianami.

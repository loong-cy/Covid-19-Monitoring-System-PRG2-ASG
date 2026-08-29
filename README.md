# COVID-19 Monitoring System
(Polytechnic Programming II module)

## Project Background

This project was developed as part of a Polytechnic Programming II module to apply Object-Oriented Programming concepts to simulate a simplified COVID-19 monitoring system implemented in Singapore.
The system models several measures used during the pandemic, focusing on **TravelEntry and Stay-Home Notice (SHN) requirements, SafeEntry, and TraceTogether**.

## How the system Works

The application tracks people through different parts of the monitoring process:

• **TravelEntry**: records a person's entry into Singapore and determines their SHN requirements. An SHN facility can be assigned and the applicable SHN charges can be calculated, where required.<br>
• **SafeEntry**: records a person's visits to registered business locations. The system checks the location's capacity during check-in and tracks both check-in and check-out records.<br>
• **TraceTogether**: in conjunction with SafeEntry, TraceTogether promotes community-driven contact to facilitate contact tracing efforts. TraceTogether tokens are also issued to residents who do not want to use their mobile phones to participate in the TraceTogether programme.<br>

## Classes

| Class | Purpose |
| :--- | :--- |
| `Person` | Abstract base class containing information of a person and related TravelEntry/SafeEntry records. |
| `Resident` | Represents a resident and extends `Person` with address, last departure date, and TraceTogether token information
| `Visitor` | Represents a visitor and extends `Person` with passport number and nationality.
| `TravelEntry` | Records a person's entry into Singapore and handles SHN duration and facility assignment.
| `SafeEntry` | Represents a person's check-in and check-out record at a business location.
| `BusinessLocation` | Represents a registered location and manages its maximum and current visitor capacity.
| `SHNFacility` | Represents a SHN facility and handles availability and transportation cost calculations.
| `TraceTogetherToken` | Represents a TraceTogether token issued to a resident and handles token replacement eligibility.

## Application Flow

The system is operated through a console-based main menu as follows:


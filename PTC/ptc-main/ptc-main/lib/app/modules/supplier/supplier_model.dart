class Supplier {
  final Vehicle vehicle;
  final List<Passenger> passengers;

  Supplier({
    required this.vehicle,
    required this.passengers,
  });

  factory Supplier.fromJson(Map<String, dynamic> json) => Supplier(
    vehicle: Vehicle.fromJson(json["vhcSpplier"]),
    passengers: List<Passenger>.from(json["psgSppliers"].map((x) => Passenger.fromJson(x))),
  );

  Map<String, dynamic> toJson() => {
    "vhcSpplier": vehicle.toJson(),
    "psgSppliers": List<dynamic>.from(passengers.map((x) => x.toJson())),
  };
}

class Vehicle {
  final int? id;
  final String vehicleId;
  final String vehicleType;
  final String company;
  final String vehicleImg;
  final int passengerCount;
  final DateTime inTime;
  final bool? flag;
  final DateTime? confirmOutTime;

  Vehicle({
    this.id,
    required this.vehicleImg,
    required this.vehicleId,
    required this.vehicleType,
    required this.company,
    required this.passengerCount,
    required this.inTime,
    this.flag,
    this.confirmOutTime,
  });

  factory Vehicle.fromJson(Map<String, dynamic> json) => Vehicle(
    id: json["id"],
    vehicleId: json["vehicleId"],
    vehicleType: json["vehicleType"],
    company: json["company"],
    vehicleImg: json["vehicleImg"],
    passengerCount: json["passangerCount"],
    inTime: DateTime.parse(json["inTime"]),
    flag: json["flag"],
    confirmOutTime: json["confirmOutTime"] == null ? null : DateTime.parse(json["confirmOutTime"]),
  );

  Map<String, dynamic> toJson() => {
    "vehicleId": vehicleId,
    "vehicleType": vehicleType,
    "company": company,
    "vehicleImg": vehicleImg,
    "passangerCount": passengerCount,
  };
}

class Passenger {
  final int? id;
  final int? vhcId;
  final String name;
  bool flag;

  Passenger({
    this.id,
    this.vhcId,
    required this.name,
    required this.flag,
  });

  factory Passenger.fromJson(Map<String, dynamic> json) => Passenger(
    id: json["id"],
    vhcId: json["vhcId"],
    name: json["name"],
    flag: json["flag"],
  );

  Map<String, dynamic> toJson() => {
    "name": name,
  };
}

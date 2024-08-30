class Contractor {
  int id;
  String regNum;
  String title;
  String compName;
  String location;
  String name;
  String nationalId;
  String start;
  String end;
  String inTime;
  String outTime;


  Contractor({
    required this.id,
    required this.regNum,
    required this.title,
    required this.compName,
    required this.location,
    required this.name,
    required this.nationalId,
    required this.start,
    required this.end,
    required this.inTime,
    required this.outTime,
  });

  factory Contractor.fromJson(Map<String, dynamic> json) => Contractor(
    id: json["id"],
    regNum: json["regNum"],
    title: json["title"],
    compName: json["compName"],
    location: json["location"],
    name: json["name"],
    nationalId: json["nationalId"],
    start: json["start"],
    end: json["end"],
    inTime: json["inTime"],
    outTime: json["outTime"],
  );

  Map<String, dynamic> toJson() => {
    "id": id,
    "regNum": regNum,
    "title": title,
    "compName": compName,
    "location": location,
    "name": name,
    "nationalId": nationalId,
    "start": start,
    "end": end,
    "inTime": inTime,
    "outTime": outTime,
  };
}

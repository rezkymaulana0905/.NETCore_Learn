class Home {
  List<Activity> logActivityPlan;
  List<Activity> logActivityIn;
  List<Activity> logActivityOut;

  Home({
    this.logActivityPlan = const [],
    this.logActivityIn = const [],
    this.logActivityOut = const [],
  });

  factory Home.fromJson(Map<String, dynamic> json) => Home(
    logActivityPlan: List<Activity>.from(json["plan"].map((x) => Activity.fromJson(x))),
    logActivityIn: List<Activity>.from(json["in"].map((x) => Activity.fromJson(x))),
    logActivityOut: List<Activity>.from(json["out"].map((x) => Activity.fromJson(x))),
  );
}

class Activity {
  final String date;
  final String name;
  final String type;
  final String detail;
  final String time;

  Activity({
    required this.date,
    required this.name,
    required this.type,
    required this.detail,
    required this.time,
  });

  factory Activity.fromJson(Map<String, dynamic> json) => Activity(
    date: json["date"],
    name: json["name"],
    type: json["type"],
    detail: json["detail"],
    time: json["time"] ?? "-",
  );
}

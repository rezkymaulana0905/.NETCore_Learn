class Guest {
  final String id;
  final String startDate;
  final String endDate;
  final String guestName;
  final String nationalId;
  final String guestCompany;
  final String guestDeptSect;
  final int total;
  final String requirement;
  final String? imageDate;
  final String metName;
  final String metDept;
  final String metSect;

  Guest({
    required this.id,
    required this.startDate,
    required this.endDate,
    required this.guestName,
    required this.nationalId,
    required this.guestCompany,
    required this.guestDeptSect,
    required this.total,
    required this.requirement,
    this.imageDate,
    required this.metName,
    required this.metDept,
    required this.metSect,
  });

  factory Guest.fromJson(Map<String, dynamic> json) => Guest(
      id: json['guest']["id"],
      startDate: json['guest']["startDate"],
      endDate: json['guest']["endDate"],
      guestName: json['guest']["guestName"],
      nationalId: json['guest']["nationalId"],
      guestCompany: json['guest']["guestCompany"],
      guestDeptSect: json['guest']["guestDeptSect"],
      total: json['guest']["total"],
      requirement: json['guest']["requirement"],
      imageDate: json['guest']["imageDate"],
      metName: json['met']["metName"],
      metDept: json['met']["metDept"],
      metSect: json['met']["metSect"],
  );

  Map<String, dynamic> toJson() => {
        "id": id,
        "startDate": startDate,
        "endDate": endDate,
        "guestName": guestName,
        "nationalId": nationalId,
        "guestCompany": guestCompany,
        "guestDeptSect": guestDeptSect,
        "total": total,
        "requirement": requirement,
        "imageDate": imageDate,
        "metName": metName,
        "metDept": metDept,
        "metSect": metSect,
      };
}

class Employee {
  final int? id;
  final int? seqNo;
  final bool? flag;
  final String? empId;
  final String? name;
  final String? department;
  final String? date;
  final String? timeOut;
  final String? timeReturn;
  final String? timeReasonId;
  final String? reason;
  final String? createBy;

  Employee({
    this.id,
    this.seqNo,
    this.flag,
    this.empId,
    this.name,
    this.department,
    this.date,
    this.timeOut,
    this.timeReturn,
    this.timeReasonId,
    this.reason,
    this.createBy,
  });

  factory Employee.fromJson(Map<String, dynamic> json) => Employee(
    id: json["id"],
    seqNo: json["seqNo"],
    flag: json["flag"],
    empId: json["empId"],
    name: json["name"],
    department: json["department"],
    date: json["date"],
    timeOut: json["timeOut"],
    timeReturn: json["timeReturn"],
    timeReasonId: json["timereasonId"],
    reason: json["reason"],
    createBy: json["createBy"],
  );

  Map<String, dynamic> toJson() => {
    "id": id,
    "seqNo": seqNo,
    "flag": flag,
    "empId": empId,
    "name": name,
    "department": department,
    "date": date,
    "timeOut": timeOut,
    "timeReturn": timeReturn,
    "timereasonId": timeReasonId,
    "reason": reason,
    "createBy": createBy,
  };
}

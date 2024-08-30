import '../../constants/constants.dart';
import '../../services/services.dart';
import 'employee.dart';

class EmployeeController extends BaseController {
  final employeeP = EmployeeProvider();
  late Employee employee;

  String transformData(String data) {
    String prefix = data[0];
    String suffix = data.substring(1);
    switch (prefix) {
      case '3' : return 'K$suffix';
      case '4' : return 'M$suffix';
      case '5' : return 'H$suffix';
    } return data;
  }

  @override
  void generateForm() {
    form.assignAll([
      {'title': "Date", 'value': employee.date},
      {'title': "NIK", 'value': employee.empId},
      {'title': "Name", 'value': employee.name},
      {'title': "Department", 'value': employee.department},
      {'title': "Time Out", 'value': employee.timeOut},
      {'title': "Time Return", 'value': employee.timeReturn},
      {'title': "Time Reason ID", 'value': employee.timeReasonId},
      {'title': "Reason", 'value': employee.reason},
      {'title': "Created By", 'value': employee.createBy},
    ]);
  }

  @override
  void generateModel(String json) {
    employee = Employee.fromJson(jsonDecode(json));
  }

  @override
  getData(String id, String mode) => employeeP.getData(transformData(id), mode);

  @override
  patchData(String mode) async {
    switch (mode) {
      case "In":
        return await employeeP.patchData("${employee.id}", mode);
      case "Out":
        return await employeeP.patchData("${employee.seqNo}", mode);
      default:
        return await employeeP.getData("${employee.empId}", mode);
    }
  }
}

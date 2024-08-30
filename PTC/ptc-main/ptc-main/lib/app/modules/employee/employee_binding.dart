import '../../constants/constants.dart';
import 'employee.dart';

class EmployeeBinding implements Bindings {
  @override
  void dependencies() {
    Get.put(EmployeeController());
  }
}
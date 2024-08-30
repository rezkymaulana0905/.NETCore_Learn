import '../../constants/constants.dart';
import '../../widgets/widgets.dart';
import 'employee.dart';

class EmployeePage extends GetView<EmployeeController> {
  const EmployeePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Obx(() => controller.form.isEmpty
        ? ScanPage(controller: controller)
        : DetailPage(controller: controller));
  }
}
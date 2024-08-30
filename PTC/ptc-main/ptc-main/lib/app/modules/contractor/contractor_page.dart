import '../../constants/constants.dart';
import '../../widgets/widgets.dart';
import 'contractor.dart';

class ContractorPage extends GetView<ContractorController> {
  const ContractorPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Obx(() => controller.form.isEmpty
        ? ScanPage(controller: controller)
        : DetailPage(controller: controller));
  }
}
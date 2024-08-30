import '../../constants/constants.dart';
import 'supplier.dart';
import 'widgets/portrait/portrait.dart';

class SupplierPagePortrait extends GetView<SupplierController> {
  const SupplierPagePortrait({super.key});

  @override
  Widget build(BuildContext context) {
    switch (Get.arguments['mode']) {
      case "In":
        controller.generateVehicleForm();
        controller.getVehicleFormController("Total Passenger").addListener(() {
          controller.generatePassengerForm();
        });
        return const SupplierInDetail();
      case "Out":
        controller.getSuppliers();
        return Obx(() => controller.supplier.value == null
            ? const SupplierOutList()
            : const SupplierOutDetail());
      default:
        return const Placeholder();
    }
  }
}
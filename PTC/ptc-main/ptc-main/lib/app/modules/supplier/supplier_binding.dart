import '../../constants/constants.dart';
import 'supplier.dart';

class SupplierBinding implements Bindings {
  @override
  void dependencies() {
    Get.put(SupplierController());
  }
}
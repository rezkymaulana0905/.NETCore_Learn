import '../../constants/constants.dart';
import 'contractor.dart';

class ContractorBinding implements Bindings {
  @override
  void dependencies() {
    Get.put(ContractorController());
  }
}
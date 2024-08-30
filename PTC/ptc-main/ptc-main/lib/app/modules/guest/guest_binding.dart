import '../../constants/constants.dart';
import 'guest.dart';

class GuestBinding implements Bindings {
  @override
  void dependencies() {
    Get.put(GuestController());
  }
}
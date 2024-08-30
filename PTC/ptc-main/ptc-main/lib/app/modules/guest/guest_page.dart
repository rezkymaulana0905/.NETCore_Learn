import '../../constants/constants.dart';
import '../../widgets/widgets.dart';
import 'guest.dart';

class GuestPage extends GetView<GuestController> {
  const GuestPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Obx(() => controller.form.isEmpty
        ? ScanPage(controller: controller)
        : DetailPage(
            controller: controller,
            photo: controller.guest.imageDate,
          ));
  }
}
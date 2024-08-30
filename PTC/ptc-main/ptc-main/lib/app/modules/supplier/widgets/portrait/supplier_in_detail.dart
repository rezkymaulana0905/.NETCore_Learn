import '../../../../constants/constants.dart';
import '../../../../widgets/widgets.dart';
import '../../supplier.dart';

class SupplierInDetail extends GetView<SupplierController> {
  const SupplierInDetail({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: MyAppBar(title: "Supplier Information", onTap: () => Get.back()),
      body: Padding(
        padding: EdgeInsets.only(bottom: baseHeight * 0.05),
        child: Column(
          children: <Widget>[
            Expanded(
              child: SingleChildScrollView(
                child: Column(
                  children: <Widget>[
                    SizedBox(height: baseHeight * 0.05),
                    _buildPhotoForm(),
                    _buildForm(forms: controller.vehicleForm),
                    _buildSectionDivider(),
                    Obx(() => _buildForm(forms: controller.passengerForm)),
                  ],
                ),
              ),
            ),
            ConfirmButton(
              width: baseWidth * 0.9,
              skipDialog: true,
              onPressed: () => controller.vehicleFormIsFilled() &&
                      controller.imageData.value != null
                  ? controller.addSupplier()
                  : controller.showFail("Failed", "Please fill all the form"),
              isLoading: controller.isLoading,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPhotoForm() {
    return Column(
      children: [
        Text(
          "Vehicle Picture",
          style: TextStyle(
            fontSize: baseHeight * 0.02,
            fontWeight: FontWeight.w600,
          ),
        ),
        InkWell(
          onTap: () => controller.pickImage(),
          borderRadius: BorderRadius.circular(15),
          child: Obx(() => PhotoForm(photo: controller.imageData.value)),
        ),
      ],
    );
  }

  Widget _buildForm({required List<dynamic> forms}) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: baseHeight * 0.025),
      child: Wrap(
        spacing: baseWidth * 0.05,
        runSpacing: baseHeight * 0.015,
        alignment: WrapAlignment.center,
        children: forms.map((form) => FormInput(form: form)).toList(),
      ),
    );
  }

  Widget _buildSectionDivider() {
    return Column(
      children: [
        const Divider(color: Colors.black, thickness: 4),
        Text(
          "Passengers",
          style: TextStyle(
            fontSize: baseHeight * 0.03,
            fontWeight: FontWeight.w600,
          ),
        ),
        const Divider(color: Colors.black, thickness: 4),
      ],
    );
  }
}

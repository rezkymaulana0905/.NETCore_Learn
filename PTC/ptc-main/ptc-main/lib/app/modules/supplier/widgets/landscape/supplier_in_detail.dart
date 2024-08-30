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
      body: Row(
        children: <Widget>[
          Expanded(
            flex: 3,
            child: Container(
              padding: EdgeInsets.symmetric(horizontal: baseHeight * 0.05),
              decoration: const BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.only(
                  topRight: Radius.circular(30),
                  bottomRight: Radius.circular(30),
                ),
                boxShadow: <BoxShadow>[
                  BoxShadow(
                    color: AppColor.supplierColor,
                    blurRadius: 1,
                    offset: Offset(3, 0),
                  ),
                ],
              ),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: <Widget>[
                  _buildPhotoForm(),
                  _buildVehicleForm(forms: controller.vehicleForm),
                  ConfirmButton(
                    width: baseWidth * 0.9,
                    skipDialog: true,
                    onPressed: () => controller.vehicleFormIsFilled() &&
                            controller.imageData.value != null
                        ? controller.addSupplier()
                        : controller.showFail(
                            "Failed", "Please fill all the form"),
                    isLoading: controller.isLoading,
                  ),
                ],
              ),
            ),
          ),
          // _buildSectionDivider(),
          Expanded(
            flex: 2,
            child: Column(
              children: [
                Text(
                  "Passengers",
                  style: TextStyle(
                    height: 2.5,
                    fontSize: baseHeight * 0.04,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                Obx(
                  () => _buildPassengerForm(forms: controller.passengerForm),
                ),
              ],
            ),
          ),
        ],
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
          child: Obx(
            () => PhotoForm(photo: controller.imageData.value),
          ),
        ),
      ],
    );
  }

  Widget _buildVehicleForm({required List<dynamic> forms}) {
    return Wrap(
      spacing: baseWidth * 0.05,
      runSpacing: baseHeight * 0.015,
      alignment: WrapAlignment.center,
      children: forms.map((form) => FormInput(form: form)).toList(),
    );
  }

  Widget _buildPassengerForm({required List<dynamic> forms}) {
    return Expanded(
      child: ListView.builder(
        itemCount: forms.length,
        itemBuilder: (context, index) {
          return Padding(
            padding: EdgeInsets.symmetric(
              vertical: baseWidth * 0.015,
              horizontal: baseHeight * 0.075,
            ),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  "${index + 1}.",
                  style: TextStyle(
                    fontSize: baseHeight * 0.04,
                    fontWeight: FontWeight.w600,
                  ),
                ),
                FormInput(
                  hintText: "Enter Name",
                  form: forms[index],
                ),
              ],
            ),
          );
        },
      ),
    );
  }
}

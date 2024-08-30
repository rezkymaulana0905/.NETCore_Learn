import '../../../../constants/constants.dart';
import '../../../../widgets/widgets.dart';
import '../../supplier.dart';

class SupplierOutDetail extends GetView<SupplierController> {
  const SupplierOutDetail({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: MyAppBar(
        title: "Supplier Detail",
        onTap: () => controller.supplier.value = null,
      ),
      body: Padding(
        padding: EdgeInsets.symmetric(
          vertical: baseHeight * 0.05,
          horizontal: baseWidth * 0.1,
        ),
        child: Row(
          children: [
            Expanded(
              child: _VehicleCard(vehicle: controller.supplier.value!.vehicle),
            ),
            SizedBox(width: baseWidth * 0.1),
            Expanded(
              child: Column(
                children: [
                  _buildPassengerSection(),
                  ConfirmButton(
                    onPressed: () => controller.confirm(),
                    isLoading: controller.isLoading,
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildPassengerSection() {
    return Expanded(
      child: Center(
        child: SingleChildScrollView(
          child: Obx(
            () => Wrap(
              spacing: baseWidth * 0.025,
              runSpacing: baseWidth * 0.025,
              children: controller.supplier.value!.passengers.map((passenger) {
                return _PassengerCard(
                  passenger: passenger,
                  onTap: () => controller.updateFlag(passenger),
                );
              }).toList(),
            ),
          ),
        ),
      ),
    );
  }
}

class _VehicleCard extends StatelessWidget {
  const _VehicleCard({required this.vehicle});

  final Vehicle vehicle;

  @override
  Widget build(BuildContext context) {
    return Container(
      height: baseHeight * 0.45,
      decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(15),
          border: Border.all(color: AppColor.supplierColor, width: 3)),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: <Widget>[
          PhotoForm(photo: base64Decode(vehicle.vehicleImg)),
          _buildVehicleInfo(),
        ],
      ),
    );
  }

  Widget _buildVehicleInfo() {
    final List<Map<String, String>> vehicleInfo = [
      {'title': "Date", 'value': vehicle.inTime.toString().substring(0, 10)},
      {'title': "Time", 'value': vehicle.inTime.toString().substring(11, 16)},
      {'title': "Vehicle ID", 'value': vehicle.vehicleId},
      {'title': "Vehicle Type", 'value': vehicle.vehicleType},
      {'title': "Company Name", 'value': vehicle.company},
    ];

    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        _buildInfo(
          vehicleInfo.map((e) => e['title']!).toList(),
          FontWeight.w600,
        ),
        SizedBox(width: baseHeight * 0.025),
        _buildInfo(
          vehicleInfo.map((e) => ": ${e['value']}").toList(),
          FontWeight.normal,
        ),
      ],
    );
  }

  Widget _buildInfo(List<String> infoList, FontWeight fontWeight) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: List.generate(
        infoList.length,
        (index) => Text(
          infoList[index],
          style: TextStyle(fontSize: baseWidth * 0.025, fontWeight: fontWeight),
        ),
      ),
    );
  }
}

class _PassengerCard extends StatelessWidget {
  const _PassengerCard({
    required this.passenger,
    required this.onTap,
  });

  final Passenger passenger;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
        decoration: BoxDecoration(
          color: passenger.flag ? AppColor.leave : Colors.white,
          borderRadius: BorderRadius.circular(15),
          boxShadow: <BoxShadow>[
            if (!passenger.flag)
              const BoxShadow(
                color: AppColor.shadow,
                blurRadius: 2,
                spreadRadius: 1,
              ),
          ],
        ),
        child: Text(
          passenger.name,
          style: TextStyle(
            color: passenger.flag ? Colors.white : Colors.black,
            fontSize: baseWidth * 0.03,
            fontWeight: FontWeight.w600,
          ),
        ),
      ),
    );
  }
}

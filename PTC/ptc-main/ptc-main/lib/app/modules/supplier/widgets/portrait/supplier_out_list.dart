import '../../../../constants/constants.dart';
import '../../../../widgets/widgets.dart';
import '../../supplier.dart';

class SupplierOutList extends GetView<SupplierController> {
  const SupplierOutList({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: MyAppBar(title: "Vehicle List", onTap: () => Get.back()),
      body: Column(
        children: <Widget>[
          SearchBox(searchController: controller.searchController),
          Container(
            decoration: const BoxDecoration(
              color: Colors.white,
              boxShadow: <BoxShadow>[
                BoxShadow(
                  color: AppColor.shadow,
                  blurRadius: 2,
                  offset: Offset(0, 4),
                ),
              ],
            ),
            padding: const EdgeInsets.symmetric(vertical: 10.0),
            child: const Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: <Widget>[
                MyTableColumn(label: "Date"),
                MyTableColumn(label: "Vehicle ID"),
                MyTableColumn(label: "Type"),
                MyTableColumn(label: "Action"),
              ],
            ),
          ),
          Expanded(
            child: Obx(() {
              if (controller.suppliers.value == null) {
                return Center(
                  child: SizedBox(
                    width: baseWidth * 0.2,
                    height: baseWidth * 0.2,
                    child: const CircularProgressIndicator(strokeWidth: 20),
                  ),
                );
              }
              return ListView.builder(
                itemCount: controller.filteredSuppliers.length,
                itemBuilder: (context, index) {
                  return MyTableRow(
                    index: index,
                    vehicle: controller.filteredSuppliers[index].vehicle,
                    action: () => controller.assignSupplier(index),
                  );
                },
              );
            }),
          ),
        ],
      ),
    );
  }
}

class MyTableColumn extends StatelessWidget {
  const MyTableColumn({super.key, required this.label});

  final String label;

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: baseWidth * 0.2,
      child: FittedBox(
        fit: BoxFit.scaleDown,
        child: Text(
          label,
          textAlign: TextAlign.center,
          style: TextStyle(
            fontSize: baseWidth * 0.04,
            fontWeight: FontWeight.w500,
          ),
        ),
      ),
    );
  }
}

class MyTableRow extends StatelessWidget {
  const MyTableRow({
    super.key,
    required this.index,
    required this.vehicle,
    required this.action,
  });

  final int index;
  final Vehicle vehicle;
  final VoidCallback action;

  @override
  Widget build(BuildContext context) {
    return Container(
      color: index.isEven ? AppColor.shadow : Colors.white,
      padding: const EdgeInsets.symmetric(vertical: 10.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: <Widget>[
          _buildRowData("${vehicle.inTime}".substring(0, 16)),
          _buildRowData(vehicle.vehicleId),
          _buildRowData(vehicle.vehicleType),
          _buildRowButton(),
        ],
      ),
    );
  }

  Widget _buildRowData(String vehicleData) {
    return SizedBox(
      width: baseWidth * 0.2,
      child: Text(
        vehicleData,
        textAlign: TextAlign.center,
        style: TextStyle(
          fontSize: baseWidth * 0.03,
          fontWeight: FontWeight.normal,
        ),
      ),
    );
  }

  Widget _buildRowButton() {
    return SizedBox(
      width: baseWidth * 0.2,
      child: MaterialButton(
        color: AppColor.leave,
        onPressed: action,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(30)),
        child: Text(
          "Confirm",
          maxLines: 1,
          style: TextStyle(
            color: Colors.white,
            fontSize: baseWidth * 0.03,
            fontWeight: FontWeight.w500,
          ),
        ),
      ),
    );
  }
}

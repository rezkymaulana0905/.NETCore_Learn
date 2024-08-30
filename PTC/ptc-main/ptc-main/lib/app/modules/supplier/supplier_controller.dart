import '../../constants/constants.dart';
import '../../services/services.dart';
import 'supplier.dart';

class SupplierController extends GetxController
    with VehicleController, PassengerController, HandlingController {
  final supplierP = SupplierProvider();

  final suppliers = Rxn<List<Supplier>>();
  final supplier = Rxn<Supplier>();
  final filteredSuppliers = <Supplier>[].obs;

  final searchController = TextEditingController();

  final isLoading = false.obs;

  @override
  void onInit() {
    super.onInit();
    searchController.addListener(() => getFilteredSuppliers());
  }

  String getSearchQuery() => searchController.text.toUpperCase();

  bool supplierIsMatched(Supplier supplier) {
    return supplier.vehicle.vehicleId.contains(getSearchQuery());
  }

  void getFilteredSuppliers() {
    if (getSearchQuery().isEmpty && suppliers.value != null) {
      filteredSuppliers.assignAll(suppliers.value!);
    } else {
      filteredSuppliers.assignAll(suppliers.value!.where((e) {
        return supplierIsMatched(e);
      }));
    }
  }

  void assignSupplier(index) => supplier.value = filteredSuppliers[index];

  Future<void> getSuppliers() async {
    isLoading.value = true;
    try {
      var data = await supplierP.getData();
      if (data.statusCode == 200) {
        suppliers.value = List<Supplier>.from(
            jsonDecode(data.body).map((json) => Supplier.fromJson(json)));
      }
      getFilteredSuppliers();
    } catch (e) {
      showFail("Error", "Something is Error");
    }
    isLoading.value = false;
  }

  Supplier setSupplier() {
    return Supplier(
      vehicle: setVehicle(),
      passengers: setPassenger(),
    );
  }

  Future<void> addSupplier() async {
    if (!passengerFormIsFilled()) {
      return showFail("Failed", "Please fill all the form");
    }

    isLoading.value = true;
    if (passengerFormIsFilled()) {
      try {
        var data = await supplierP.postData(setSupplier().toJson());
        if (data.statusCode == 200) {
          Get.back(result: "Supplier added");
        } else {
          var body = jsonDecode(data.body);
          body['title'] != null
              ? showFail(body['title'], "")
              : showFail("Error", "Error Occurred");
        }
      } catch (e) {
        showFail("Error", "Something is Error");
      }
    }
    isLoading.value = false;
  }

  Future<void> confirm() async {
    isLoading.value = true;
    try {
      var data = await supplierP.patchData(setSupplierLeave());
      if (data.statusCode == 200) {
        Get.back(result: "Supplier confirmed");
      } else {
        var body = jsonDecode(data.body);
        body['title'] != null
            ? showFail(body['title'], "")
            : showFail("Error", "Error Occurred");
      }
    } catch (e) {
      showFail("Error", "Error Occurred");
    }
    isLoading.value = false;
  }

  Map setSupplierLeave() {
    return {
      "vhcId": supplier.value!.vehicle.id,
      "psgId": List<int>.from(supplier.value!.passengers
          .where((passenger) => passenger.flag)
          .map((passenger) => passenger.id)),
    };
  }

  void updateFlag(Passenger passenger) {
    passenger.flag = !passenger.flag;
    supplier.refresh();
  }

  bool passengerFormIsFilled() => formIsFilled(passengerForm);

  bool vehicleFormIsFilled() => formIsFilled(vehicleForm);

  bool formIsFilled(form) {
    for (var item in form) {
      if (item['controller'].text == "") return false;
    }
    return true;
  }

  @override
  int getTotalPassenger() {
    return int.tryParse(getVehicleFormController("Total Passenger").text) ?? 0;
  }
}

mixin VehicleController {
  final vehicleForm = [].obs;
  final imageData = Rxn<Uint8List>();
  final imagePicker = ImagePicker();

  Future<void> pickImage() async {
    XFile? image = await imagePicker.pickImage(source: ImageSource.camera);
    imageData.value = await image?.readAsBytes();
  }

  void generateVehicleForm() {
    vehicleForm.assignAll([
      {
        'title': "Vehicle ID",
        'controller': TextEditingController(),
        'textCase': TextCapitalization.characters,
        'length': 11,
        'pattern': r'^[a-zA-Z]{0,2}\d{0,4}[a-zA-Z]{0,3}',
      },
      {
        'title': "Vehicle Type",
        'controller': TextEditingController(),
      },
      {
        'title': "Company Name",
        'controller': TextEditingController(),
      },
      {
        'title': "Total Passenger",
        'controller': TextEditingController(),
        'formType': TextInputType.number,
        'length': 1,
      },
    ]);
  }

  TextEditingController getVehicleFormController(String title) {
    return vehicleForm.firstWhere((e) => e['title'] == title)['controller'];
  }

  Vehicle setVehicle() {
    return Vehicle(
      vehicleId: getVehicleFormController("Vehicle ID").text,
      vehicleType: getVehicleFormController("Vehicle Type").text,
      company: getVehicleFormController("Company Name").text,
      vehicleImg: base64Encode(imageData.value!),
      passengerCount: getTotalPassenger(),
      inTime: DateTime.now(),
    );
  }

  int getTotalPassenger();
}

mixin PassengerController {
  final passengerForm = [].obs;

  void generatePassengerForm() {
    passengerForm.assignAll(RxList.generate(getTotalPassenger(),
        (_) => {'title': "Name", 'controller': TextEditingController()}));
  }

  List<Passenger> setPassenger() {
    return List<Passenger>.from(passengerForm.map((element) {
      return Passenger(name: element['controller'].text, flag: false);
    }));
  }

  int getTotalPassenger();
}

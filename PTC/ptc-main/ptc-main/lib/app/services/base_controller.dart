import '../constants/constants.dart';
import 'package:http/http.dart' as http;

import 'services.dart';

abstract class BaseController extends GetxController with HandlingController{
  late FlutterDataWedge fdw;
  final scannerMode = false.obs;
  final error = ''.obs;
  final isLoading = false.obs;
  final form = [].obs;

  @override
  void onInit() async {
    super.onInit();
    initScanner();
  }

  @override
  void onClose() {
    fdw.scannerControl(false);
  }

  void changeScannerMode() {
    scannerMode.value = !scannerMode.value;
    fdw.scannerControl(scannerMode.value);
    resetError();
  }

  Future<void> initScanner() async {
    fdw = FlutterDataWedge(profileName: 'PTC');
    fdw.onScanResult.listen((result) {
      fetchData(result.data, Get.arguments['mode']);
      scannerMode.value = false;
    });
    await fdw.initialize();
  }

  void generateForm();

  void generateModel(String json);

  Future<http.Response> getData(String id, String mode);

  Future<http.Response> patchData(String mode);

  Future<void> fetchData(String id, String mode) async {
    isLoading.value = true;
    try {
      var data = await getData(id, mode);
      if (data.statusCode == 200) {
        generateModel(data.body);
        generateForm();
      } else {
        var body = jsonDecode(data.body);
        body['title'] != null
            ? showError(body['title'])
            : showError("Error Occurred");
      }
    } catch (e) {
      showError("Error Occurred");
    }
    isLoading.value = false;
  }

  Future<void> confirm(String mode) async {
    isLoading.value = true;
    try {
      var data = await patchData(mode);
      if (data.statusCode == 200) {
        isLoading.value = false;
        Get.back(result: "Confirmed");
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

  bool isError() => error.value.isNotEmpty;
  void showError(message) => error.value = message;
  void resetError() => error.value = '';
}

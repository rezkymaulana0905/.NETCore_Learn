import '../../constants/constants.dart';
import '../../routes/routes.dart';
import '../../services/services.dart';
import 'home.dart';

class HomeController extends GetxController
    with
        MenuController,
        ModeController,
        LogActivityController,
        HandlingController {
  final homeP = HomeProvider();
  final home = Home().obs;

  final isLoading = false.obs;

  @override
  void onInit() {
    super.onInit();
    setMenuButton();
    mode.value = modes[0];
    getLogActivity();
  }

  Future<void> getLogActivity() async {
    isLoading.value = true;
    try {
      var data = await homeP.getData();
      if (data.statusCode == 200) {
        home.value = Home.fromJson(jsonDecode(data.body));
      }
    } catch (e) {
      showFail("Error", "Something is Error");
    }
    isLoading.value = false;
  }

  Future<void> logout() async {
    final prefs = await SharedPreferences.getInstance();
    prefs.remove('token');
    Get.offNamed(Routes.login);
  }
}

mixin LogActivityController {
  final List<Map<String, dynamic>> sortOrder = [
    {'mode': "Descending", 'icon': Icons.arrow_downward},
    {'mode': "Ascending", 'icon': Icons.arrow_upward},
  ];

  int changeSortOrder(List<Activity> activities, int index) {
    switch (sortOrder[index]['mode']) {
      case "Ascending":
        activities.sort((a, b) => b.time.compareTo(a.time));
      case "Descending":
        activities.sort((a, b) => a.time.compareTo(b.time));
    }
    return (index + 1) % sortOrder.length;
  }

  Color getTypeColor(String menuType) {
    switch (menuType) {
      case "Employee":
        return AppColor.employeeColor;
      case "Guest":
        return AppColor.guestColor;
      case "Contractor":
        return AppColor.contractorColor;
      case "Supplier":
        return AppColor.supplierColor;
      default:
        return AppColor.off;
    }
  }
}

mixin MenuController {
  final List<Map<String, dynamic>> menu = [
    {'title': "Employee", 'route': Routes.employee},
    {'title': "Guest", 'route': Routes.guest},
    {'title': "Contractor", 'route': Routes.contractor},
    {'title': "Supplier", 'route': Routes.supplier},
  ];

  void setMenuButton() {
    for (var item in menu) {
      item['icon'] = "assets/icons/${item['title'].toLowerCase()}_icon.png";
      item['state'] = false.obs;
    }
  }

  void changeMenuState(Map menu) => menu['state'].value = !menu['state'].value;
}

mixin ModeController {
  final List<Map<String, dynamic>> modes = [
    {'mode': "In", 'color': AppColor.enter, 'position': Alignment.centerLeft},
    {'mode': "Out", 'color': AppColor.leave, 'position': Alignment.centerRight},
  ];

  final RxMap<String, dynamic> mode = RxMap();

  void changeMode() => mode.value = modes[(getModeIndex() + 1) % modes.length];

  int getModeIndex() => modes.indexOf(mode.value);
}

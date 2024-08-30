import '../constants/constants.dart';
import '../modules/modules.dart';
import 'routes.dart';

abstract class AppPages {
  static final pages = [
    GetPage(name: Routes.initial, page: () => SplashScreen()),
    GetPage(
      name: Routes.login,
      page: () => OrientationBuilder(
        builder: (context, orientation) {
          _screenDimensionInit(orientation);
          return switch (orientation) {
            Orientation.portrait => const LoginPagePortrait(),
            Orientation.landscape => const LoginPageLandscape(),
          };
        },
      ),
      binding: LoginBinding(),
    ),
    GetPage(
      name: Routes.home,
      page: () => OrientationBuilder(
        builder: (context, orientation) {
          _screenDimensionInit(orientation);
          return switch (orientation) {
            Orientation.portrait => const HomePagePortrait(),
            Orientation.landscape => const HomePageLandscape(),
          };
        },
      ),
      binding: HomeBinding(),
    ),
    GetPage(
      name: Routes.employee,
      page: () => const EmployeePage(),
      binding: EmployeeBinding(),
    ),
    GetPage(
      name: Routes.guest,
      page: () => const GuestPage(),
      binding: GuestBinding(),
    ),
    GetPage(
      name: Routes.contractor,
      page: () => const ContractorPage(),
      binding: ContractorBinding(),
    ),
    GetPage(
      name: Routes.supplier,
      page: () => OrientationBuilder(
        builder: (context, orientation) {
          _screenDimensionInit(orientation);
          return switch (orientation) {
            Orientation.portrait => const SupplierPagePortrait(),
            Orientation.landscape => const SupplierPageLandscape(),
          };
        },
      ),
      binding: SupplierBinding(),
    ),
  ];

  static void _screenDimensionInit(Orientation orientation) {
    switch (orientation) {
      case Orientation.portrait:
        baseWidth = Get.width;
        baseHeight = Get.height;
      case Orientation.landscape:
        baseWidth = Get.height;
        baseHeight = Get.width;
    }
  }
}

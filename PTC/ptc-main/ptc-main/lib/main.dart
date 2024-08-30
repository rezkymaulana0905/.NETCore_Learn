import 'app/constants/constants.dart';
import 'app/routes/routes.dart';
import 'app/routes/pages.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: 'PTC',
      getPages: AppPages.pages,
      initialRoute: Routes.initial,
      debugShowCheckedModeBanner: false,
      theme: ThemeData(fontFamily: "Poppins"),
    );
  }
}
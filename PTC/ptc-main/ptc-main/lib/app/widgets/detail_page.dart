import '../constants/constants.dart';
import '../services/services.dart';
import 'widgets.dart';

class DetailPage extends StatelessWidget {
  const DetailPage({super.key, required this.controller, this.photo});

  final BaseController controller;
  final String? photo;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      bottomNavigationBar: Padding(
        padding: EdgeInsets.only(
          bottom: baseHeight * 0.05,
          left: baseHeight * 0.05,
          right: baseHeight * 0.05,
        ),
        child: ConfirmButton(
          onPressed: () => controller.confirm(Get.arguments['mode']),
          isLoading: controller.isLoading,
        ),
      ),
      appBar: MyAppBar(
        title: "${Get.arguments['menu']} Detail",
        onTap: () => Get.back(),
      ),
      body: Center(
        child: SingleChildScrollView(
          child: _DetailBody(forms: controller.form, photo: photo),
        ),
      ),
    );
  }
}

class _DetailBody extends StatelessWidget {
  const _DetailBody({required this.forms, this.photo});

  final List<dynamic> forms;
  final String? photo;

  @override
  Widget build(BuildContext context) {
    return photo == null ? _noPhotoBuild() : _hasPhotoBuild();
  }

  Widget _noPhotoBuild() {
    return Builder(builder: (context) {
      final size = switch (context.orientation) {
        Orientation.portrait => Size(baseWidth * 0.05, baseHeight * 0.015),
        Orientation.landscape => Size(baseWidth * 0.05, baseHeight * 0.025),
      };
      return Wrap(
        spacing: size.width,
        runSpacing: size.height,
        alignment: WrapAlignment.center,
        runAlignment: WrapAlignment.center,
        crossAxisAlignment: WrapCrossAlignment.end,
        children: forms.map((form) {
          return form['controller'] == null
              ? FormValue(form: form)
              : FormInput(form: form);
        }).toList(),
      );
    });
  }

  Widget _hasPhotoBuild() {
    return Builder(builder: (context) {
      final size = switch (context.orientation) {
        Orientation.portrait => Size(double.infinity, Get.height * 0.75),
        Orientation.landscape => Size(double.infinity, Get.height * 0.65),
      };
      final photoSize = switch (context.orientation) {
        Orientation.portrait => Size(baseHeight * 0.2, baseHeight * 0.25),
        Orientation.landscape => Size(baseHeight * 0.2, baseHeight * 0.15),
      };
      return Container(
        alignment: Alignment.center,
        width: size.width,
        height: size.height,
        child: Wrap(
          spacing: baseWidth * 0.005,
          runSpacing: baseHeight * 0.025,
          direction: Axis.vertical,
          alignment: WrapAlignment.end,
          crossAxisAlignment: WrapCrossAlignment.center,
          children: [
            SizedBox(
              height: photoSize.height,
              width: photoSize.width,
              child: Expanded(child: PhotoForm(photo: base64Decode(photo!))),
            ),
            ...forms.map(
              (form) => form['controller'] == null
                  ? FormValue(form: form)
                  : FormInput(form: form),
            ),
          ],
        ),
      );
    });
  }
}

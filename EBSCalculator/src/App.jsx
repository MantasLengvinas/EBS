import "./App.css";
import { useEffect, useState } from "react";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import {
  Box,
  TextField,
  MenuItem,
  Grid,
  FormGroup,
  FormControlLabel,
  Checkbox,
  Radio,
  RadioGroup,
  Slider,
  Input,
} from "@mui/material";

export default function App() {
  const darkTheme = createTheme({
    palette: {
      mode: "dark",
    },
  });

  const marks = [
    {
      value: 425,
      label: "425kWh",
    },
    {
      value: 850,
      label: "850kWh",
    },
    {
      value: 1700,
      label: "1700kWh",
    },
  ];

  const [rates, setRates] = useState([]);
  const [providers, setProviders] = useState([]);
  const [provider, setProvider] = useState("");
  const [isFixedTerm, setIsFixedTerm] = useState(false);
  const [electricityUsageAmbiguous, setElectricityUsageAmbiguous] =
    useState(850);
  const [electricityUsageDay, setElectricityUsageDay] = useState(100);
  const [electricityUsageEvening, setElectricityUsageEvening] = useState(100);
  const [electricityUsageNight, setElectricityUsageNight] = useState(100);
  const [timezonePlan, setTimezonePlan] = useState(1);
  const [price, setPrice] = useState(0);

  const fetchRatesData = (id) => {
    fetch(`${import.meta.env.VITE_BASE_URL}/tariff/prognosis?providerId=${id}`)
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        setRates(data.data);
      });
  };

  const fetchProvidersData = () => {
    fetch(`${import.meta.env.VITE_BASE_URL}/provider`)
      .then((response) => {
        return response.json();
      })
      .then((data) => {
        setProviders(data.data);
      });
  };

  useEffect(() => {
    fetchProvidersData();
  }, []);

  useEffect(() => {
    setTimeout(() => {
      setPrice(
        calculatePrice(
          electricityUsageAmbiguous,
          electricityUsageDay,
          electricityUsageEvening,
          electricityUsageNight,
          timezonePlan
        )
      );
    }, 100);
  }, [
    electricityUsageAmbiguous,
    electricityUsageDay,
    electricityUsageEvening,
    electricityUsageNight,
    timezonePlan,
    rates,
  ]);

  function calculatePrice(
    electricityUsageAmbiguous,
    electricityUsageDay,
    electricityUsageEvening,
    electricityUsageNight,
    timezonePlan
  ) {
    var sum = 0;
    if (timezonePlan == 1) {
      rates.forEach((rate) => {
        sum += rate.rateAmbiguous * electricityUsageAmbiguous;
      });
    }
    if (timezonePlan == 2) {
      rates.forEach((rate) => {
        sum += rate.rateDay * electricityUsageDay;
        sum += rate.rateEvening * electricityUsageEvening;
      });
    }
    if (timezonePlan == 3) {
      rates.forEach((rate) => {
        sum += rate.rateDay * electricityUsageDay;
        sum += rate.rateEvening * electricityUsageEvening;
        sum += rate.rateNight * electricityUsageNight;
      });
    }

    return Math.round((sum + Number.EPSILON) * 100) / 100;
  }

  return (
    <ThemeProvider theme={darkTheme}>
      <FormGroup>
        <Grid
          gap={2}
          margin={2}
          padding={2}
          container
          direction="column"
          className="bg-slate-800 text-white rounded-lg"
          maxWidth="sm"
          width="auto"
        >
          <Grid item>
            <Box className="text-purple-200 font-medium mb-4">
              Numatytas tiekėjas
            </Box>
            <TextField
              value={provider}
              label="Pasirinkite numatytą tiekėją"
              select
              className="w-64"
              onChange={(e) => {
                setProvider(e.target.value);
                fetchRatesData(e.target.value);
              }}
            >
              {providers.map((provider) => (
                <MenuItem key={provider.providerId} value={provider.providerId}>
                  {provider.providerName}
                </MenuItem>
              ))}
            </TextField>
          </Grid>
          <Grid item>
            <Box className="text-purple-200 font-medium">
              Laiko juostų planas
            </Box>
            <RadioGroup
              onChange={(e) => setTimezonePlan(e.target.value)}
              value={timezonePlan}
            >
              <FormControlLabel
                value={1}
                control={<Radio size="small" />}
                label="Vienos laiko juostos planas"
              ></FormControlLabel>
              <FormControlLabel
                value={2}
                control={<Radio size="small" />}
                label="Dviejų laiko juostų planas"
              ></FormControlLabel>
              <FormControlLabel
                value={3}
                control={<Radio size="small" />}
                label="Trijų laiko juostų planas"
              ></FormControlLabel>
            </RadioGroup>
          </Grid>
          <Grid item>
            <Box className="text-purple-200 font-medium">
              Terminuotos sutarties sudarymas
            </Box>
            <FormControlLabel
              label="Ar norite sudaryti terminuotą sutartį?"
              control={<Checkbox />}
              checked={isFixedTerm}
              onChange={(e) => setIsFixedTerm(e.target.checked)}
            ></FormControlLabel>
          </Grid>
          {timezonePlan == 1 ? (
            <Grid item>
              <Box className="text-purple-200 font-medium">
                Kiek suvartojate elektros (kWh) per mėnesį?
              </Box>
              <Grid container paddingTop={2}>
                <Grid item sm>
                  <Slider
                    marks={marks}
                    min={100}
                    max={2000}
                    value={electricityUsageAmbiguous}
                    onChange={(e) =>
                      setElectricityUsageAmbiguous(e.target.value)
                    }
                  />
                </Grid>
                <Grid item sm paddingLeft={4}>
                  <Input
                    size="small"
                    onChange={(e) =>
                      setElectricityUsageAmbiguous(e.target.value)
                    }
                    value={electricityUsageAmbiguous}
                    inputProps={{
                      step: 50,
                      min: 100,
                      max: 2000,
                      type: "number",
                    }}
                  />
                </Grid>
              </Grid>
            </Grid>
          ) : (
            ""
          )}
          {timezonePlan == 2 ? (
            <Grid item>
              <Box className="text-purple-200 font-medium">
                Kiek suvartojate elektros (kWh) per mėnesį dienos metu?
              </Box>
              <Grid container paddingTop={2}>
                <Grid item sm>
                  <Slider
                    min={100}
                    max={2000}
                    value={electricityUsageDay}
                    onChange={(e) => setElectricityUsageDay(e.target.value)}
                  />
                </Grid>
                <Grid item sm paddingLeft={4}>
                  <Input
                    size="small"
                    onChange={(e) => setElectricityUsageDay(e.target.value)}
                    value={electricityUsageDay}
                    inputProps={{
                      step: 50,
                      min: 100,
                      max: 2000,
                      type: "number",
                    }}
                  />
                </Grid>
              </Grid>
              <Grid item>
                <Box className="text-purple-200 font-medium">
                  Kiek suvartojate elektros (kWh) per mėnesį vakaro metu?
                </Box>
                <Grid container paddingTop={2}>
                  <Grid item sm>
                    <Slider
                      min={100}
                      max={2000}
                      value={electricityUsageEvening}
                      onChange={(e) =>
                        setElectricityUsageEvening(e.target.value)
                      }
                    />
                  </Grid>
                  <Grid item sm paddingLeft={4}>
                    <Input
                      size="small"
                      onChange={(e) =>
                        setElectricityUsageEvening(e.target.value)
                      }
                      value={electricityUsageEvening}
                      inputProps={{
                        step: 50,
                        min: 100,
                        max: 2000,
                        type: "number",
                      }}
                    />
                  </Grid>
                </Grid>
              </Grid>
            </Grid>
          ) : (
            ""
          )}
          {timezonePlan == 3 ? (
            <Grid item>
              <Box className="text-purple-200 font-medium">
                Kiek suvartojate elektros (kWh) per mėnesį dienos metu?
              </Box>
              <Grid container paddingTop={2}>
                <Grid item sm>
                  <Slider
                    min={100}
                    max={2000}
                    value={electricityUsageDay}
                    onChange={(e) => setElectricityUsageDay(e.target.value)}
                  />
                </Grid>
                <Grid item sm paddingLeft={4}>
                  <Input
                    size="small"
                    onChange={(e) => setElectricityUsageDay(e.target.value)}
                    value={electricityUsageDay}
                    inputProps={{
                      step: 50,
                      min: 100,
                      max: 2000,
                      type: "number",
                    }}
                  />
                </Grid>
              </Grid>
              <Grid item>
                <Box className="text-purple-200 font-medium">
                  Kiek suvartojate elektros (kWh) per mėnesį vakaro metu?
                </Box>
                <Grid container paddingTop={2}>
                  <Grid item sm>
                    <Slider
                      min={100}
                      max={2000}
                      value={electricityUsageEvening}
                      onChange={(e) =>
                        setElectricityUsageEvening(e.target.value)
                      }
                    />
                  </Grid>
                  <Grid item sm paddingLeft={4}>
                    <Input
                      size="small"
                      onChange={(e) =>
                        setElectricityUsageEvening(e.target.value)
                      }
                      value={electricityUsageEvening}
                      inputProps={{
                        step: 50,
                        min: 100,
                        max: 2000,
                        type: "number",
                      }}
                    />
                  </Grid>
                </Grid>
              </Grid>
              <Grid item>
                <Box className="text-purple-200 font-medium">
                  Kiek suvartojate elektros (kWh) per mėnesį nakties metu?
                </Box>
                <Grid container paddingTop={2}>
                  <Grid item sm>
                    <Slider
                      min={100}
                      max={2000}
                      value={electricityUsageNight}
                      onChange={(e) => setElectricityUsageNight(e.target.value)}
                    />
                  </Grid>
                  <Grid item sm paddingLeft={4}>
                    <Input
                      size="small"
                      onChange={(e) => setElectricityUsageNight(e.target.value)}
                      value={electricityUsageNight}
                      inputProps={{
                        step: 50,
                        min: 100,
                        max: 2000,
                        type: "number",
                      }}
                    />
                  </Grid>
                </Grid>
              </Grid>
            </Grid>
          ) : (
            ""
          )}
          <Grid item>
            <Box className="text-purple-200 font-medium">
              Jūsų prognozuojamos elektros išlaidos
            </Box>
            <Grid
              className="p-4"
              container
              direction="row"
              justifyContent="space-between"
            >
              <Grid item>
                <Grid
                  container
                  className="text-purple-400 font-bold text-xl"
                  direction="column"
                  alignItems="center"
                >
                  <Grid item id="sildymoKaina">
                    {price}
                  </Grid>
                  € per metus, su PVM
                </Grid>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
      </FormGroup>
    </ThemeProvider>
  );
}
